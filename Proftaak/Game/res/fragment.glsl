#version 400

uniform mat4 u_cameraTransformation;
uniform samplerBuffer u_voxelBuffer;
uniform ivec3 u_voxelGridSize;

uniform float u_zoom; // = (screenHeight * 0.5) / tan(fov * 0.5)

struct Ray {
	vec3 origin, direction;
};

struct HitData {
	float dist;
	vec3 normal;
};

bool IsVoxel(int x, int y, int z){
	return u_voxelBuffer[x + y * u_voxelGridSize.x + z * u_voxelGridSize.x * u_voxelGridSize.y] != 0;
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
		if(isVoxel(floor(point).xyz)) {
			return HitData(dist, vec3(0.0));
		}
	}
}

void main ()
{
	// Generate a local ray and transform it to world space
	Ray ray = generateRay();
	ray.origin = (u_cameraTransformation * vec4(ray.origin, 1.0)).xyz;
	ray.direction = (u_cameraTransformation * vec4(ray.direction, 0.0)).xyz;

	float dist = trace(ray).dist;

	gl_FragColor = vec4(0.7, 1.0 / dist, 0.8, 1.0);
}