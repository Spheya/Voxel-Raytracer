#version 450

#include "math.glsl"

#define WORLD_RENDER_DISTANCE 256 * 1.5
#define RENDER_DISTANCE 256
#define MAX_MODELS 512
#define MODEL_DATA_STRIDE 4

uniform samplerBuffer u_voxelBuffer;
uniform samplerBuffer u_modelData;

uniform vec2 u_windowSize;
uniform float u_zoom;
uniform float f;

out vec4 colour;

int getVoxelData(ivec3 pos, int modelIndex) {
	int w = floatBitsToInt(texelFetch(u_modelData, MODEL_DATA_STRIDE * modelIndex + 1).r);
	int h = floatBitsToInt(texelFetch(u_modelData, MODEL_DATA_STRIDE * modelIndex + 2).r);
	int d = floatBitsToInt(texelFetch(u_modelData, MODEL_DATA_STRIDE * modelIndex + 3).r);

	if (pos.x >= w || pos.y >= h || pos.z >= d ||
		pos.x < 0 || pos.y < 0 || pos.z < 0)
		return 0;

	return floatBitsToInt(texelFetch(u_voxelBuffer, pos.x + pos.y * w + pos.z * w * d).r);
}

Ray generateRay() {
	return Ray(vec3(0,0,0), normalize(vec3(gl_FragCoord.xy - u_windowSize * 0.5, u_zoom)));
}

HitData traceModel(Ray ray, int modelIndex) {
	vec3 point = ray.origin;
	ivec3 gridPoint = ivec3(floor(point));

	ivec3 mapPos = ivec3(floor(ray.origin));
	vec3 deltaDist = abs(vec3(length(ray.direction)) / ray.direction);
	ivec3 rayStep = ivec3(sign(ray.direction));
	vec3 sideDist = (sign(ray.direction) * (vec3(mapPos) - ray.origin) + (sign(ray.direction) * 0.5) + 0.5) * deltaDist;
	bvec3 mask;
	int material;

	for(int i = 0; i < RENDER_DISTANCE; i++){
		material = getVoxelData(mapPos, modelIndex);
		if (material != 0) {
			vec3 normal;
			if (mask.x)
				normal = vec3(-sign(ray.direction.x), 0.0, 0.0);
			if (mask.y)
				normal = vec3(0.0, -sign(ray.direction.y), 0.0);
			if (mask.z)
				normal = vec3(0.0, 0.0, -sign(ray.direction.z));

			return HitData(1.0, normal, 0);
		}

		mask = lessThanEqual(sideDist.xyz, min(sideDist.yzx, sideDist.zxy));
		sideDist += vec3(mask) * deltaDist;
		mapPos += ivec3(mask) * rayStep;
	}

	return HitData(WORLD_RENDER_DISTANCE, vec3(-1.0), 0);
}

HitData trace(Ray ray) {
	int nModels = floatBitsToInt(texelFetch(u_modelData, 0).r);
	HitData result = HitData(WORLD_RENDER_DISTANCE, vec3(-1.0), 0);

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
	ray.origin = vec3(-5.0, -5.0, 0.0);

	// Find the hitpoint of the ray
	HitData hit = trace(ray);

	// Display the normal
	colour = vec4(hit.normal.xyz * 0.5 + 0.5, 1.0);
	if(hit.dist == WORLD_RENDER_DISTANCE) colour.rgb = vec3(0.7, 0.9, 1.0) + ray.direction.y*0.8;
}
