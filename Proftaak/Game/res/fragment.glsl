#version 450

uniform samplerBuffer u_voxelBuffer;
uniform ivec3 u_bufferDimensions;

out vec4 colour;

vec4 isVoxel(int x, int y, int z) {
	return texelFetch(u_voxelBuffer, x + y * u_bufferDimensions.x + z * u_bufferDimensions.x * u_bufferDimensions.y);
}

void main () {
	//bool draw = isVoxel(int(gl_FragCoord.x), int(gl_FragCoord.y), 0);

	colour = vec4(float(floatBitsToInt(isVoxel(int(gl_FragCoord.x), int(gl_FragCoord.y), 0).r)), 0.0, 0.8, 1.0);
}
