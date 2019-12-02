#pragma once

#define WORLD_RENDER_DISTANCE 4096.0
#define RENDER_DISTANCE 256
#define MAX_MODELS 512

struct Ray {
	vec3 origin, direction;
};

struct HitData {
	float dist;
	float ambientOcclusion;
	vec3 normal;
	int material;
};


int getVoxelData(in samplerBuffer voxelBuffer, in ivec4 modelData, in ivec3 pos) {
	return floatBitsToInt(texelFetch(voxelBuffer, pos.x + pos.y * modelData.x + pos.z * modelData.x * modelData.y + modelData.w).r);
}

int getSafeVoxelData(in samplerBuffer voxelBuffer, in ivec4 modelData, in ivec3 pos) {
	if (any(greaterThanEqual(pos, modelData.xyz)) || any(lessThan(pos, ivec3(0))))
		return 0;
	return floatBitsToInt(texelFetch(voxelBuffer, pos.x + pos.y * modelData.x + pos.z * modelData.x * modelData.y + modelData.w).r);
}

float sum(vec3 v) { return dot(v, vec3(1.0)); }

float vertexAmbientOcclusion(bvec2 side, bool corner) {
	return (float(side.x) + float(side.y) + float(corner)) / 3.0;
}

vec4 voxelAmbientOcclusion(in samplerBuffer voxelBuffer, in ivec4 modelData, in vec3 pos, in vec3 d1, in vec3 d2) {
	bvec4 side = bvec4(bool(getSafeVoxelData(voxelBuffer, modelData, ivec3(pos + d1))),
		bool(getSafeVoxelData(voxelBuffer, modelData, ivec3(pos + d2))),
		bool(getSafeVoxelData(voxelBuffer, modelData, ivec3(pos - d1))),
		bool(getSafeVoxelData(voxelBuffer, modelData, ivec3(pos - d2))));
	bvec4 corner = bvec4(bool(getSafeVoxelData(voxelBuffer, modelData, ivec3(pos + d1 + d2))),
		bool(getSafeVoxelData(voxelBuffer, modelData, ivec3(pos - d1 + d2))),
		bool(getSafeVoxelData(voxelBuffer, modelData, ivec3(pos - d1 - d2))),
		bool(getSafeVoxelData(voxelBuffer, modelData, ivec3(pos + d1 - d2))));

	vec4 ao;
	ao.x = vertexAmbientOcclusion(side.xy, corner.x);
	ao.y = vertexAmbientOcclusion(side.yz, corner.y);
	ao.z = vertexAmbientOcclusion(side.zw, corner.z);
	ao.w = vertexAmbientOcclusion(side.wx, corner.w);
	return 1.0 - ao;
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
	ray.origin = (worldToObject * vec4(ray.origin, 1.0)).xyz + vec3(modelData.xyz) * 0.5;
	ray.direction = (worldToObject * vec4(ray.direction, 0.0)).xyz;

	vec3 invertedDirection = 1.0 / ray.direction;

	// Check if the ray hits the model
	vec3 modelHit1 = (vec3(0.0) - vec3(ray.origin)) * invertedDirection;
	vec3 modelHit2 = (vec3(modelData.xyz) - vec3(ray.origin)) * invertedDirection;
	float modelHitNear = max(max(min(modelHit1.x, modelHit2.x), min(modelHit1.y, modelHit2.y)), min(modelHit1.z, modelHit2.z));
	float modelHitFar = min(min(max(modelHit1.x, modelHit2.x), max(modelHit1.y, modelHit2.y)), max(modelHit1.z, modelHit2.z));

	if(any(lessThan(vec2(modelHitFar), vec2(modelHitNear, 0.0))))
		return HitData(WORLD_RENDER_DISTANCE, 0.0, vec3(-1.0), 0);

	// Setup variables to traverse through the grid
	vec3 mapPos = floor(ray.origin + ray.direction * max(modelHitNear - 0.1, 0.0));
	vec3 deltaDist = abs(vec3(length(ray.direction)) * invertedDirection);
	vec3 rayStep = sign(ray.direction);
	vec3 sideDist = (sign(ray.direction) * (mapPos - ray.origin) + (sign(ray.direction) * 0.5) + 0.5) * deltaDist;
	vec3 mask = step(sideDist.xyz, sideDist.yzx) * step(sideDist.xyz, sideDist.zxy);

	sideDist += mask * deltaDist;
	mapPos += mask * rayStep;;

	int material;

	// Make sure we are in the model (to avoid artifacts)
	for(int i = 0; i < 2; i++){
		if (!(any(greaterThanEqual(mapPos, vec3(modelData.xyz))) || any(lessThan(mapPos, vec3(0.0))))) continue;

		mask = step(sideDist.xyz, sideDist.yzx) * step(sideDist.xyz, sideDist.zxy);
		sideDist += mask * deltaDist;
		mapPos += mask * rayStep;
	}

	// Traverse through the grid
	for(int i = 0; i < RENDER_DISTANCE; i++){
		// Stop checking after leaving the grid
		if (any(greaterThanEqual(mapPos, vec3(modelData.xyz))) || any(lessThan(mapPos, vec3(0.0))))
			return HitData(WORLD_RENDER_DISTANCE, 0.0, vec3(-1.0), 0);

		material = getVoxelData(voxelBuffer, modelData, ivec3(mapPos));
		if (material != 0) {
			vec3 normal = normalize((objectToWorldNormal * vec4(vec3(mask) * -sign(ray.direction), 0.0)).xyz);

			// Calculate the distance to the hitpoint
			vec3 hit1 = (vec3(mapPos) - vec3(ray.origin)) * invertedDirection;
			vec3 hit2 = (vec3(mapPos + 1.0) - vec3(ray.origin)) * invertedDirection;
			float hit = max(max(min(hit1.x, hit2.x), min(hit1.y, hit2.y)), min(hit1.z, hit2.z));
			float worldHit = length((objectToWorld * vec4(ray.direction * hit, 0.0)).xyz);

			// Make a rough approximation for the ambient occlusion
			vec3 endRayPos;
			vec2 uv;
			vec4 ambient;

			ambient = voxelAmbientOcclusion(voxelBuffer, modelData, mapPos - rayStep * mask, mask.zxy, mask.yzx);
			endRayPos = ray.origin + ray.direction * hit;

			uv = mod(vec2(dot(mask * endRayPos.yzx, vec3(1.0)), dot(mask * endRayPos.zxy, vec3(1.0))), vec2(1.0));

			float interpAo = mix(mix(ambient.z, ambient.w, uv.x), mix(ambient.y, ambient.x, uv.x), uv.y);
			interpAo = pow(interpAo, 0.4);

			return HitData(worldHit, interpAo, normal, material);
		}

		// Move to the next cell in the grid
		mask = step(sideDist.xyz, sideDist.yzx) * step(sideDist.xyz, sideDist.zxy);
		sideDist += mask * deltaDist;
		mapPos += mask * rayStep;
	}

	return HitData(WORLD_RENDER_DISTANCE, 0.0, vec3(-1.0), 0);
}

HitData trace(in samplerBuffer voxelBuffer, in samplerBuffer modelDataBuffer, in samplerBuffer modelTransformsBuffer, in Ray ray) {
	int nModels = floatBitsToInt(texelFetch(modelDataBuffer, 0).r);
	HitData result = HitData(WORLD_RENDER_DISTANCE, 0.0, vec3(-1.0), 0);

	// Check for intersection with every model
	for(int i = 0; i < MAX_MODELS; ++i){
		if(i > nModels) break;

		HitData newResult = traceModel(voxelBuffer, modelDataBuffer, modelTransformsBuffer, ray, i);
		result = (newResult.dist < result.dist) ? newResult : result;
	}

	return result;
}
