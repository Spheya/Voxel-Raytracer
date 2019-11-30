#pragma once

#include "raytracing.glsl"
#include "material.glsl"

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

vec3 spheyaShading(vec3 lambertSum, Ray ray, Material material, vec3 normal, vec3 reflectiveColour, vec3 refractiveColour) {
	// Calculate the reflective coefficient from the refractionIndex
	float reflectiveCoefficient = (1.0 - material.refractiveIndex) / (1.0 + material.refractiveIndex);
	reflectiveCoefficient *= reflectiveCoefficient;

	// Calculate the amount of reflection and refraction
	float reflectiveAmount = fresnelSchlick(-dot(ray.direction, normal), reflectiveCoefficient);
	float refractiveAmount = 1.0 - reflectiveAmount;

	// The amount of light that gets scattered when refracting
	float scatterAmount = 1.0;

	return reflectiveAmount * reflectiveColour + 
		   refractiveAmount * (
				scatterAmount * lambertSum +
				(1.0 - scatterAmount) * refractiveColour
		   );
}

vec3 lambertShading(Material material, vec3 normal, vec3 lightDir, vec3 lightColour) {
	float diffuse = max(dot(lightDir, normal), 0.0);
	return material.baseColour * lightColour * diffuse;
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
			 Ray ray,
			 HitData hit,
			 vec3 reflectiveColour,
			 vec3 refractiveColour) {

	//Testing variables
	Material material = Material(
		vec3(0.453, 0.742, 0.551),	// base colour
		1.2						// refractive index
	);

	vec3 lightColour = vec3(1.0, 1.0, 1.0);
	float lightIntensity = 1.0;
	vec3 lightDir = normalize(vec3(-0.5, 1.5, -1.0));
	vec3 hitPos = ray.origin + ray.direction * hit.dist;

	// Calculate the amount of light that hits the object
	float attenuation = softshadow(voxelBuffer, modelDataBuffer, modelTransformsBuffer, hitPos, lightDir) * hit.ambientOcclusion;

	// Calculate the colour using a lighting model
	vec3 lambertSum = lambertShading(material, hit.normal, lightDir, lightColour * lightIntensity); // Do this for every light source
	vec3 final = spheyaShading(lambertSum * attenuation, ray, material, hit.normal, reflectiveColour, refractiveColour); // Do this once
	
	return final;
}