#version 450

#include "lighting.glsl"
#include "math.glsl"

#define WORLD_RENDER_DISTANCE 256 * 1.5
#define RENDER_DISTANCE 256
#define MAX_MODELS 512
#define MODEL_DATA_STRIDE 4

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

int getVoxelData(in ivec3 pos, in ivec4 modelData) {
	return floatBitsToInt(texelFetch(u_voxelBuffer, pos.x + pos.y * modelData.x + pos.z * modelData.x * modelData.y + modelData.w).r);
}

Ray generateRay() {
	return Ray((u_camera.matrix * vec4(0,0,0,1)).xyz, (u_camera.matrix * vec4(normalize(vec3(gl_FragCoord.xy - u_windowSize * 0.5, u_camera.zoom)), 0.0)).xyz);
}

HitData traceModel(in Ray ray, in int modelIndex) {
	ivec4 modelData = floatBitsToInt(texelFetch(u_modelData, modelIndex + 1));

	mat4 worldToObject = mat4(
		texelFetch(u_modelTransformations, modelIndex * 12 + 0),
		texelFetch(u_modelTransformations, modelIndex * 12 + 1),
		texelFetch(u_modelTransformations, modelIndex * 12 + 2),
		texelFetch(u_modelTransformations, modelIndex * 12 + 3)
	);

	mat4 objectToWorld = mat4(
		texelFetch(u_modelTransformations, modelIndex * 12 + 4),
		texelFetch(u_modelTransformations, modelIndex * 12 + 5),
		texelFetch(u_modelTransformations, modelIndex * 12 + 6),
		texelFetch(u_modelTransformations, modelIndex * 12 + 7)
	);

	mat4 objectToWorldNormal = mat4(
		texelFetch(u_modelTransformations, modelIndex * 12 + 8),
		texelFetch(u_modelTransformations, modelIndex * 12 + 9),
		texelFetch(u_modelTransformations, modelIndex * 12 + 10),
		texelFetch(u_modelTransformations, modelIndex * 12 + 11)
	);

	// Transform the ray to object space
	ray.origin = (worldToObject * vec4(ray.origin, 1.0)).xyz + modelData.xyz * 0.5;
	ray.direction = (worldToObject * vec4(ray.direction, 0.0)).xyz;

	vec3 invertedDirection = 1.0 / ray.direction;

	// Check if the ray hits the model
	vec3 modelHit1 = (vec3(0.0) - vec3(ray.origin)) * invertedDirection;
	vec3 modelHit2 = (vec3(modelData.xyz) - vec3(ray.origin)) * invertedDirection;
	float modelHitNear = max(max(min(modelHit1.x, modelHit2.x), min(modelHit1.y, modelHit2.y)), min(modelHit1.z, modelHit2.z));
	float modelHitFar = min(min(max(modelHit1.x, modelHit2.x), max(modelHit1.y, modelHit2.y)), max(modelHit1.z, modelHit2.z));

	if(any(lessThan(vec2(modelHitFar), vec2(modelHitNear, 0.0))))
		return HitData(WORLD_RENDER_DISTANCE, vec3(-1.0), 0);

	// Traverse through the voxel grid
	ivec3 mapPos = ivec3(floor(ray.origin + ray.direction * max(modelHitNear - 0.00001, 0.0)));
	vec3 deltaDist = abs(vec3(length(ray.direction)) * invertedDirection);
	ivec3 rayStep = ivec3(sign(ray.direction));
	vec3 sideDist = (sign(ray.direction) * (vec3(mapPos) - ray.origin) + (sign(ray.direction) * 0.5) + 0.5) * deltaDist;
	bvec3 mask = lessThanEqual(sideDist.xyz, min(sideDist.yzx, sideDist.zxy));
	sideDist += vec3(mask) * deltaDist;
	mapPos += ivec3(mask) * rayStep;

	int material;

	for(int i = 0; i < RENDER_DISTANCE; i++){
		if (any(greaterThanEqual(mapPos, modelData.xyz)) || any(lessThan(mapPos, ivec3(0))))
			return HitData(WORLD_RENDER_DISTANCE, vec3(-1.0), 0);

		material = getVoxelData(mapPos, modelData);
		if (material != 0) {
			vec3 normal = normalize((objectToWorldNormal * vec4(ivec3(mask) * -sign(ray.direction), 0.0)).xyz);

			// Calculate the distance to the hitpoint
			vec3 hit1 = (vec3(mapPos) - vec3(ray.origin)) * invertedDirection;
			vec3 hit2 = (vec3(mapPos + 1) - vec3(ray.origin)) * invertedDirection;
			float hit = max(max(min(hit1.x, hit2.x), min(hit1.y, hit2.y)), min(hit1.z, hit2.z));

			float worldHit = length((objectToWorld * vec4(ray.direction * hit, 0.0)).xyz);

			return HitData(worldHit, normal, 0);
		}

		// Move to the next cell in the grid
		mask = lessThanEqual(sideDist.xyz, min(sideDist.yzx, sideDist.zxy));
		sideDist += vec3(mask) * deltaDist;
		mapPos += ivec3(mask) * rayStep;
	}

	return HitData(WORLD_RENDER_DISTANCE, vec3(-1.0), 0);
}

HitData trace(in Ray ray) {
	int nModels = floatBitsToInt(texelFetch(u_modelData, 0).r);
	HitData result = HitData(WORLD_RENDER_DISTANCE, vec3(-1.0), 0);

	// Check for intersection with every model
	for(int i = 0; i < MAX_MODELS; ++i){
		if(i > nModels) break;

		HitData newResult = traceModel(ray, i);
		result = (newResult.dist < result.dist) ? newResult : result;
	}

	return result;
}

void main () {
	// Generate a local ray and transform it to world space
	Ray ray = generateRay();

	ray.origin = vec3(0.0, 0.0, -32.0);

	// Find the hitpoint of the ray
	HitData hit = trace(ray);

	// Display the normal
	colour = vec4(hit.normal.xyz * 0.5 + 0.5, 1.0);
	colour.b = 1.0 - colour.b;
	if(hit.dist == WORLD_RENDER_DISTANCE) colour.rgb = vec3(0.7, 0.9, 1.0) + ray.direction.y*0.8;
}
