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

	float intensity;
	vec3 colour;
};

#define MAX_DIR_LIGHTS 8
#define MAX_POINT_LIGHTS 32

uniform DirectionalLight u_dirLights[MAX_DIR_LIGHTS];
uniform int u_dirLightCount;
uniform PointLight u_pointLights[MAX_POINT_LIGHTS];
uniform int u_pointLightCount;

float fresnelSchlick(float cos_theta, float reflectiveIndex) {
	float p = 1.0 - cos_theta;
	return reflectiveIndex + (1.0 - reflectiveIndex) * p * p * p * p * p;
}

vec3 spheyaShading(vec3 lambertSum, Ray ray, Material material, vec3 normal, vec3 reflectiveColour, vec3 refractiveColour) {
	// Calculate the reflective coefficient from the refractionIndex
	float reflectiveCoefficient = (1.0 - material.refractiveIndex) / (1.0 + material.refractiveIndex);
	reflectiveCoefficient *= reflectiveCoefficient;

	// Calculate the amount of reflection and refraction
	float reflectiveAmount = fresnelSchlick(-dot(ray.direction, normal), reflectiveCoefficient) * float(material.refractiveIndex > 1.0);
	float refractiveAmount = 1.0 - reflectiveAmount;

	// The amount of light that gets scattered when refracting
	float scatterAmount = 1.0;

	return reflectiveAmount * reflectiveColour +
		   refractiveAmount * (
				scatterAmount * lambertSum +
				(1.0 - scatterAmount) * refractiveColour
		   );
}

vec3 lambertShading(Material material, vec3 normal, vec3 lightDir, vec3 lightColour, float attenuation) {
	float diffuse = max(dot(lightDir, normal), 0.0);
	return material.baseColour * lightColour * diffuse * attenuation;
}

float softshadow(vec3 hitpos, vec3 lightDir, float maxDist) {

	Ray ray = Ray(hitpos + lightDir * 0.0002, lightDir);
	float hit = traceCheap(ray);

	return float(hit >= maxDist);
}

vec3 shading(Ray ray, HitData hit, Material material, vec3 reflectiveColour, vec3 refractiveColour) {

	// Calculate the colour using a lighting model
	vec3 hitPos = ray.origin + ray.direction * hit.dist;
	vec3 lambertSum = vec3(0.0);
	for (int i = 0; i < MAX_DIR_LIGHTS; ++i) {
		if (i >= u_dirLightCount) break;
		vec3 lightDir = normalize(u_dirLights[i].direction); //u_dirLights[i].direction
		vec3 lightColour = u_dirLights[i].colour;
		float lightIntensity = u_dirLights[i].intensity;

		float attenuation = softshadow(hitPos, lightDir, WORLD_RENDER_DISTANCE);
		lambertSum += lambertShading(material, hit.normal, lightDir, lightColour * lightIntensity, attenuation);
	}
	for (int i = 0; i < MAX_POINT_LIGHTS; ++i) {
		if (i >= u_pointLightCount) break;

		vec3 lightDir = u_pointLights[i].position - hitPos;
		float lightDist = length(lightDir);
		lightDir = normalize(lightDir);
		vec3 lightColour = u_pointLights[i].colour;
		float lightIntensity = u_pointLights[i].intensity / ((lightDist * lightDist) * 0.01);

		float attenuation = softshadow(hitPos, lightDir, distance(u_pointLights[i].position, ray.origin));
		lambertSum += lambertShading(material, hit.normal, lightDir, lightColour * lightIntensity, attenuation);
	}

	lambertSum *= hit.ambientOcclusion;

	// lambertSum += vec3(0.05, 0.1, 0.15); // Do this for every ambient light source

	return spheyaShading(lambertSum, ray, material, hit.normal, reflectiveColour, refractiveColour); // Do this once
}
