#version 450

uniform samplerBuffer u_voxelBuffer;
uniform ivec3 u_bufferDimensions;

uniform float u_zoom;

out vec4 colour;

struct Ray {
	vec3 origin, direction;
};

struct HitData {
	float dist;
	vec3 normal;
	int material;
};

int getVoxelData(int x, int y, int z) {
	return (x > u_bufferDimensions.x || y > u_bufferDimensions.y || z > u_bufferDimensions.z) ?
				floatBitsToInt(texelFetch(u_voxelBuffer, x + y * u_bufferDimensions.x + z * u_bufferDimensions.x * u_bufferDimensions.y)) : 
				0;
}

Ray generateRay() {
	return Ray(vec3(0,0,0), normalize(vec3(gl_FragCoord.xy, u_zoom)));
}

HitData trace(Ray ray) {
	// Yes yEs, dis si raytreecing
	vec3 point = ray.origin;
	float dist = 0.0;
	for(int i = 0; i < 100; ++i){
		point += ray.direction;
		dist += 1.0f;
		int material = getVoxelData(int(floor(point).x), int(floor(point).y), int(floor(point).z));
		if(material != 0) {
			return HitData(dist, vec3(0.0), material);
		}
	}

	return HitData(-1.0, vec3(0.0), 0);
}


void main () {
	// Generate a local ray and transform it to world space
	Ray ray = generateRay();
	//ray.origin = (u_cameraTransformation * vec4(ray.origin, 1.0)).xyz;
	//ray.direction = (u_cameraTransformation * vec4(ray.direction, 0.0)).xyz;

	float dist = trace(ray).dist;

	// gl_FragColor = vec4(0.7, 1.0 / dist, 0.8, 1.0);
	gl_FragColor = vec4(1.0, vec2(0.0), 1.0);
}