#version 450

#include "raytracing.glsl"
#include "lighting.glsl"
#include "material.glsl"

#define RAY_RECURSION 0

struct Camera {
	mat4 matrix;
	float zoom;
};

uniform vec2 u_windowSize;
uniform Camera u_camera;

out vec4 colour;

Ray generateRay() {
	return Ray((u_camera.matrix * vec4(0,0,0,1)).xyz, (u_camera.matrix * vec4(normalize(vec3((gl_FragCoord.xy + vec2(0.0, int(gl_FragCoord.x) & 1)) - u_windowSize * 0.5, u_camera.zoom)), 0.0)).xyz);
}

vec3 backgroundColour(vec3 direction) {
	return vec3(0.7, 0.9, 1.0) + direction.y*0.8;
}

void main () {
	colour.a = 1.0;

	Ray reflectionRays[RAY_RECURSION + 1];
	HitData reflectionHits[RAY_RECURSION + 1];

	// TODO: disable warnings, so we don't have to initialize this data beforehand
	for(int i = 0; i < RAY_RECURSION + 1; ++i) {
		reflectionRays[i] = Ray(vec3(0.0), vec3(0.0));
		reflectionHits[i] = HitData(WORLD_RENDER_DISTANCE, 0.0, vec3(-1.0), 0);
	}
	// NOTE: works fine for me without this shit, but doesn't affect performance at all it seems

	reflectionRays[0] = generateRay();
	reflectionHits[0] = trace(reflectionRays[0]);

	vec3 reflectionBackground;

	// Setup rays
	int reflections = RAY_RECURSION;
	for(int i = 1; i < RAY_RECURSION + 1; ++i) {
		// Setup a reflection ray
		reflectionRays[i] = Ray(reflectionRays[i-1].origin + reflectionRays[i-1].direction * reflectionHits[i-1].dist,
			reflectionRays[i-1].direction - 2.0 * reflectionHits[i-1].normal * dot(reflectionRays[i-1].direction, reflectionHits[i-1].normal));
		reflectionHits[i] = trace(reflectionRays[i]);

		if(reflectionHits[i].dist >= WORLD_RENDER_DISTANCE || u_materials[reflectionHits[i].material].refractiveIndex < 1.0) {
			reflections = i;
			break;
		}
	}

	// Calculate the colours for the rays
	colour.rgb = backgroundColour(reflectionRays[RAY_RECURSION].direction);

	for(int i = RAY_RECURSION; i >= 0; --i) {
		if(i > reflections) continue;

		colour.rgb = shading(reflectionRays[i], reflectionHits[i], u_materials[reflectionHits[i].material], colour.rgb, vec3(0.0));
		if(reflectionHits[i].dist == WORLD_RENDER_DISTANCE) colour.rgb = backgroundColour(reflectionRays[i].direction);
	}
}
