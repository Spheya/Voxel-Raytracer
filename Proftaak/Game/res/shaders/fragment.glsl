#version 450

#include "raytracing.glsl"
#include "lighting.glsl"

struct Camera {
	mat4 matrix;
	float zoom;
};

uniform samplerBuffer u_voxelBuffer;
uniform samplerBuffer u_modelData;
uniform samplerBuffer u_modelTransformations;

uniform vec2 u_windowSize;
uniform Camera u_camera;

out vec4 colour;

Ray generateRay() {
	return Ray((u_camera.matrix * vec4(0,0,0,1)).xyz, (u_camera.matrix * vec4(normalize(vec3(gl_FragCoord.xy - u_windowSize * 0.5, u_camera.zoom)), 0.0)).xyz);
}

void main () {
	// Generate a local ray and transform it to world space
	Ray ray = generateRay();

	ray.origin += vec3(0.0001, 0.0001, 0.0001);

	// Find the hitpoint of the ray
	HitData hit = trace(u_voxelBuffer, u_modelData, u_modelTransformations, ray);

	// Calculate the colour
	colour.a = 1.0;
	colour.rgb = shading(u_voxelBuffer, u_modelData, u_modelTransformations, ray, hit);
	//colour.rgb *= hit.ambientOcclusion;
	if(hit.dist == WORLD_RENDER_DISTANCE) colour.rgb = vec3(0.7, 0.9, 1.0) + ray.direction.y*0.8;
}
