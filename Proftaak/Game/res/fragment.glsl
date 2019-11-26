#version 450

#include "math.glsl"

uniform samplerBuffer u_voxelBuffer;
uniform ivec3 u_bufferDimensions;
uniform vec2 u_windowSize;
uniform float u_zoom;
uniform float f;

out vec4 colour;

int getVoxelData(int x, int y, int z) {
	if (x >= u_bufferDimensions.x || y >= u_bufferDimensions.y || z >= u_bufferDimensions.z || x < 0 || y < 0 || z < 0)
		return 0;
		
	return floatBitsToInt(texelFetch(u_voxelBuffer, x + y * u_bufferDimensions.x + z * u_bufferDimensions.x * u_bufferDimensions.y).r);
}

Ray generateRay() {
	return Ray(vec3(0,0,0), normalize(vec3(gl_FragCoord.xy - u_windowSize * 0.5, u_zoom)));
}

HitData trace(Ray ray) {
	// Yes yEs, dis si raytreecing
	vec3 point = ray.origin;
	float dist = 0.0;
	for(int i = 0; i < 500; ++i){
		point += ray.direction;
		dist += 0.2f;
		int material = getVoxelData(int(floor(point.x)), int(floor(point.y)), int(floor(point.z)));
		if(material != 0) {
			return HitData(dist, vec3(0.0), material);
		}
	}

	return HitData(-1.0, vec3(0.0), 0);
}


void main () {
	// Generate a local ray and transform it to world space
	Ray ray = generateRay();
	ray.origin = vec3(0.0, 0.0, -f * 8);
	//ray.origin = (u_cameraTransformation * vec4(ray.origin, 1.0)).xyz;
	//ray.direction = (u_cameraTransformation * vec4(ray.direction, 0.0)).xyz;

	float dist = trace(ray).dist;

	colour = vec4(1.0, 1.0 / dist, 1.0, 1.0);
	if(dist < 0) colour.rgb = vec3(0.7, 0.9, 1.0) + ray.direction.y*0.8;
}
