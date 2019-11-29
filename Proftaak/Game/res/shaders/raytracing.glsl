#pragma once

#define WORLD_RENDER_DISTANCE 4096
#define RENDER_DISTANCE 256
#define MAX_MODELS 512

struct Ray {
	vec3 origin, direction;
};

struct HitData {
	float dist;
	vec3 normal;
	int material;
};

int getVoxelData(in samplerBuffer voxelBuffer, in ivec3 pos, in ivec4 modelData) {
	return floatBitsToInt(texelFetch(voxelBuffer, pos.x + pos.y * modelData.x + pos.z * modelData.x * modelData.y + modelData.w).r);
}

HitData traceModel(in samplerBuffer voxelBuffer, 
				   in samplerBuffer modelDataBuffer,
				   in samplerBuffer modelTransformsBuffer,
				   in Ray ray,
				   in int modelIndex) {

	ivec4 modelData = floatBitsToInt(texelFetch(modelDataBuffer, modelIndex + 1));

	mat4 worldToObject = mat4(
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 0),
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 1),
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 2),
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 3)
	);

	mat4 objectToWorld = mat4(
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 4),
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 5),
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 6),
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 7)
	);

	mat4 objectToWorldNormal = mat4(
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 8),
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 9),
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 10),
		texelFetch(modelTransformsBuffer, modelIndex * 12 + 11)
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
	ivec3 mapPos = ivec3(floor(ray.origin + ray.direction * max(modelHitNear - 0.1, 0.0)));
	vec3 deltaDist = abs(vec3(length(ray.direction)) * invertedDirection);
	ivec3 rayStep = ivec3(sign(ray.direction));
	vec3 sideDist = (sign(ray.direction) * (vec3(mapPos) - ray.origin) + (sign(ray.direction) * 0.5) + 0.5) * deltaDist;
	bvec3 mask = lessThanEqual(sideDist.xyz, min(sideDist.yzx, sideDist.zxy));
	sideDist += vec3(mask) * deltaDist;
	mapPos += ivec3(mask) * rayStep;

	for(int i = 0; i < 2; i++){
		if (!(any(greaterThanEqual(mapPos, modelData.xyz)) || any(lessThan(mapPos, ivec3(0)))))
			break;

		mask = lessThanEqual(sideDist.xyz, min(sideDist.yzx, sideDist.zxy));
		sideDist += vec3(mask) * deltaDist;
		mapPos += ivec3(mask) * rayStep;
	}
	
	int material;

	for(int i = 0; i < RENDER_DISTANCE; i++){
		if (any(greaterThanEqual(mapPos, modelData.xyz)) || any(lessThan(mapPos, ivec3(0))))
			return HitData(WORLD_RENDER_DISTANCE, vec3(-1.0), 0);

		material = getVoxelData(voxelBuffer, mapPos, modelData);
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

HitData trace(in samplerBuffer voxelBuffer, in samplerBuffer modelDataBuffer, in samplerBuffer modelTransformsBuffer, in Ray ray) {
	int nModels = floatBitsToInt(texelFetch(modelDataBuffer, 0).r);
	HitData result = HitData(WORLD_RENDER_DISTANCE, vec3(-1.0), 0);

	// Check for intersection with every model
	for(int i = 0; i < MAX_MODELS; ++i){
		if(i > nModels) break;

		HitData newResult = traceModel(voxelBuffer, modelDataBuffer, modelTransformsBuffer, ray, i);
		result = (newResult.dist < result.dist) ? newResult : result;
	}

	return result;
}