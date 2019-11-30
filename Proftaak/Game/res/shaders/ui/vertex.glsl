#version 450

uniform mat4 u_projectionMatrix;
uniform mat4 u_modelMatrix;

in vec2 position;

void main(void)
{
	gl_Position = u_projectionMatrix * u_modelMatrix * vec4(position, 0.0, 1.0);
}
