#version 450

uniform mat4 u_projectionMatrix;
uniform mat4 u_modelMatrix;

out vec2 uvCoords;

in vec2 position;

void main(void)
{
	uvCoords = position + 0.5;
	uvCoords.y = 1.0 - uvCoords.y;
	uvCoords = clamp(uvCoords, 0.001, 0.999);

	gl_Position = u_projectionMatrix * u_modelMatrix * vec4(position, 0.0, 1.0);
}
