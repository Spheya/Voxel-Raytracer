﻿#version 450

layout(location = 0) in vec3 position;

uniform mat4 u_modelMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

void main(void)
{
	gl_Position = u_projectionMatrix * u_viewMatrix * u_modelMatrix * vec4(position, 1.0);
}