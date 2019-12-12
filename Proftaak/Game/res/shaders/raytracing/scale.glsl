#version 450

uniform sampler2D u_framebuffer;
uniform vec2 u_resolution;

out vec4 colour;

void main() {
  ivec2 bufSize = textureSize(u_framebuffer, 1);
  vec2 pixelStride = u_resolution / bufSize;

  vec2 uvCoords = gl_FragCoord.xy / pixelStride;
  if ((int(uvCoords.x) & 1) == 0) {
    uvCoords.y -= 1;
  }

  colour = texture(u_framebuffer, uvCoords / bufSize);
}
