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

vec3 backgroundColour(vec3 direction){
	return vec3(0.7, 0.9, 1.0) + direction.y*0.8;
}

void main () {
	// Trace a ray
	Ray ray = generateRay();
	HitData hit = trace(u_voxelBuffer, u_modelData, u_modelTransformations, ray);

	// Trace a reflection ray
	Ray reflectionRay = Ray(ray.origin + ray.direction * hit.dist, ray.direction - 2.0 * hit.normal * dot(ray.direction, hit.normal));
	//reflectionRay.origin += reflectionRay.direction * 0.01;
	HitData reflectionHit = trace(u_voxelBuffer, u_modelData, u_modelTransformations, reflectionRay);

	vec3 reflectionBackground = backgroundColour(reflectionRay.direction);
	vec3 reflectionColour = shading(u_voxelBuffer, u_modelData, u_modelTransformations, reflectionRay, reflectionHit, reflectionBackground, reflectionBackground);
	if(reflectionHit.dist == WORLD_RENDER_DISTANCE) reflectionColour.rgb = reflectionBackground;

	// Calculate the colour
	colour.a = 1.0;
	vec3 background = backgroundColour(ray.direction);
	colour.rgb = shading(u_voxelBuffer, u_modelData, u_modelTransformations, ray, hit, reflectionColour, background);
	if(hit.dist == WORLD_RENDER_DISTANCE) colour.rgb = background;
}
