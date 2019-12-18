#version 450

uniform sampler2D u_framebuffer;
uniform vec2 u_resolution;

out vec4 colour;

vec2 getUvCoords(vec2 pixelCoords, vec2 pixelSize){
	return (pixelCoords - pixelSize * 0.5 * vec2(0.0, int(pixelCoords.x) & 1)) / u_resolution;
}

void main() {
  ivec2 bufSize = textureSize(u_framebuffer, 0);
  vec2 pixelSize = u_resolution / bufSize;

  vec2 uvCoords = getUvCoords(gl_FragCoord.xy, pixelSize);

  vec4 blendColour = 0.25 * (texture(u_framebuffer, getUvCoords(gl_FragCoord.xy + vec2(1.0, 0.0), pixelSize))
						   + texture(u_framebuffer, getUvCoords(gl_FragCoord.xy - vec2(1.0, 0.0), pixelSize))
						   + texture(u_framebuffer, getUvCoords(gl_FragCoord.xy + vec2(0.0, 1.0), pixelSize))
						   + texture(u_framebuffer, getUvCoords(gl_FragCoord.xy - vec2(0.0, 1.0), pixelSize)));
  vec4 bufferColour = texture(u_framebuffer, uvCoords);

  colour = mix(bufferColour, blendColour, ((int(gl_FragCoord.x) + int(gl_FragCoord.y)) & 1));

}
