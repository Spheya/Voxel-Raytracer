#version 450

uniform vec4 u_colour;
uniform sampler2D u_texture;

in vec2 uvCoords;

out vec4 colour;

void main()
{
    colour = texture(u_texture, uvCoords) * u_colour;
}
