#pragma once

#include "raytracing.glsl"

struct DirectionalLight {
	vec3 direction;

	float intensity;
	vec3 colour;
};

struct PointLight {
	vec3 position;
	vec3 direction;

	float intensity;
	vec3 colour;
};

float fresnelSchlick(float cos_theta, float reflectiveIndex) {
	return reflectiveIndex + (1.0 - reflectiveIndex) * pow(1.0 - cos_theta, 5.0);
}

vec3 spheyaLighting(in samplerBuffer voxelBuffer, 
					in samplerBuffer modelDataBuffer,
					in samplerBuffer modelTransformsBuffer,
					vec3 viewDir,
					vec3 normal,
					vec3 lightDir,
					vec3 hitPos,
					vec3 baseColour,
					vec3 lightColour,
					float attenuation,
					float refractionIndex) {

	// Diffuse lighting is lambert shading
	float diffuse = max(dot(lightDir, normal), 0.0);

	// Calculate the reflective coefficient from the refractionIndex
	float reflectiveCoefficient = (1.0 - refractionIndex) / (1.0 + refractionIndex);
	reflectiveCoefficient *= reflectiveCoefficient;

	// Calculate the amount of reflection and refraction
	float reflectiveAmount = fresnelSchlick(-dot(viewDir, normal), reflectiveCoefficient);
	float refractiveAmount = 1.0 - reflectiveAmount;

	// Trace the colours for refraction and reflection
	vec3 reflectiveColour = lightColour;
	vec3 refractiveColour = vec3(0.0);

	// The amount of light that gets scattered when refracting
	float scatterAmount = 1.0;

	return baseColour * (reflectiveAmount * reflectiveColour
		   + refractiveAmount * (1.0 - scatterAmount) * refractiveColour
		   + refractiveAmount * scatterAmount * diffuse * attenuation * lightColour);
}

float softshadow(in samplerBuffer voxelBuffer, 
				 in samplerBuffer modelDataBuffer,
				 in samplerBuffer modelTransformsBuffer,
				 in vec3 hitpos, 
				 in vec3 lightDir) {

	Ray ray = Ray(hitpos + lightDir * 0.01, lightDir);
	HitData hit = trace(voxelBuffer, modelDataBuffer, modelTransformsBuffer, ray);

	if (hit.dist < WORLD_RENDER_DISTANCE) {
		return 0.0;
	} else {
		return 1.0;
	}
	return float(hit.dist <  WORLD_RENDER_DISTANCE);
}

vec3 shading(in samplerBuffer voxelBuffer, 
			 in samplerBuffer modelDataBuffer,
			 in samplerBuffer modelTransformsBuffer,
			 vec3 v,
			 vec3 n,
			 vec3 hitpos) {

	//Testing variables
	float refractionIndex = 2.4;
	vec3 lightColour = vec3(1.0, 1.0, 1.0);
	vec3 baseColour = vec3(0.453, 0.742, 0.551);
	float lightIntensity = 1.0;
	vec3 lightdir = normalize(vec3(-0.5, 1.5, -1.0));

	// Calculate the amount of light that hits the object
	// TODO: Ambient occlusion
	float attenuation = softshadow(voxelBuffer, modelDataBuffer, modelTransformsBuffer, hitpos, lightdir);

	// Calculate the colour using a lighting model
	vec3 final = spheyaLighting(voxelBuffer, modelDataBuffer, modelTransformsBuffer, v, n, lightdir, hitpos, baseColour, lightColour * lightIntensity, attenuation, refractionIndex);
  
	return final;
}