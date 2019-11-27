#version 450

#include "math.glsl"

#define RENDER_DISTANCE 256
#define MAX_MODELS 512

struct ModelSpace {
	int offset;
	ivec3 size;
	mat4 modelMatrix;
	mat4 normalMatrix;
};

uniform samplerBuffer u_voxelBuffer;
// uniform ivec3 u_bufferDimensions;
uniform vec2 u_windowSize;
uniform float u_zoom;
uniform float f;

uniform ModelSpace u_models[MAX_MODELS];

out vec4 colour;

int getVoxelDataIndexed(ivec3 pos, int modelIndex) {
	if (pos.x >= u_models[modelIndex].size.x || pos.y >= u_models[modelIndex].size.y || pos.z >= u_models[modelIndex].size.z || pos.x < 0 || pos.y < 0 || pos.z < 0)
		return 0;
	return floatBitsToInt(texelFetch(u_voxelBuffer, pos.x + pos.y * u_models[modelIndex].size.x + pos.z * u_models[modelIndex].size.x * u_models[modelIndex].size.y).r);
}

int getVoxelData(ivec3 pos) {
	for (int i = 0; i < MAX_MODELS; i++) {
		int result = getVoxelDataIndexed(pos, i);
		if (result > 0)
			return result;
	}

	return 0;
}

Ray generateRay() {
	return Ray(vec3(0,0,0), normalize(vec3(gl_FragCoord.xy - u_windowSize * 0.5, u_zoom)));
}

HitData trace(Ray ray) {
	// Yes yEs, dis si raytreecing
	vec3 point = ray.origin;
	ivec3 gridPoint = ivec3(floor(point));

	ivec3 mapPos = ivec3(floor(ray.origin));
	vec3 deltaDist = abs(vec3(length(ray.direction)) / ray.direction);
	ivec3 rayStep = ivec3(sign(ray.direction));
	vec3 sideDist = (sign(ray.direction) * (vec3(mapPos) - ray.origin) + (sign(ray.direction) * 0.5) + 0.5) * deltaDist;
	bvec3 mask;
	int material;

	for(int i = 0; i < RENDER_DISTANCE; i++){
		material = getVoxelData(mapPos);
		if (material != 0) {
			vec3 normal;

			if (mask.x) {
				normal = vec3(-sign(ray.direction.x), 0.0, 0.0);
			}
			if (mask.y) {
				normal = vec3(0.0, -sign(ray.direction.y), 0.0);
			}
			if (mask.z) {
				normal = vec3(0.0, 0.0, -sign(ray.direction.z));
			}

			return HitData(1.0, normal, 0);
		}

		mask = lessThanEqual(sideDist.xyz, min(sideDist.yzx, sideDist.zxy));
		sideDist += vec3(mask) * deltaDist;
		mapPos += ivec3(mask) * rayStep;
	}

	return HitData(-1.0, vec3(-1.0), 0);
}


void main () {
	// Generate a local ray and transform it to world space
	Ray ray = generateRay();
	ray.origin = vec3(-5.0, -5.0 + f, -f);
	//ray.origin = (u_cameraTransformation * vec4(ray.origin, 1.0)).xyz;
	//ray.direction = (u_cameraTransformation * vec4(ray.direction, 0.0)).xyz;

	HitData hit = trace(ray);

	colour = vec4(hit.normal.xyz * 0.5 + 0.5, 1.0);
	if(hit.dist < 0) colour.rgb = vec3(0.7, 0.9, 1.0) + ray.direction.y*0.8;


	//colour = vec4(1.0, 0.0, 1.0, 1.0);
}
