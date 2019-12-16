#version 450

uniform sampler2D u_framebuffer;
uniform vec2 u_resolution;

out vec4 colour;

void main() {
  ivec2 bufSize = textureSize(u_framebuffer, 1);
  vec2 pixelStride = u_resolution / bufSize;
  vec2 pixelSize = 1.0 / pixelStride;

  vec2 uvCoords = gl_FragCoord.xy / pixelStride;
  if ((int(gl_FragCoord.x) & 2) == 0) {
    uvCoords.y += pixelSize.y;
  }

  colour = texture(u_framebuffer, (uvCoords / bufSize));
  // colour = texture(u_framebuffer, gl_FragCoord.xy / u_resolution);
  // colour = vec4(1.0);

  // colour = vec4(uvCoords, 0.0, 1.0);

  // colour = vec4(gl_FragCoord.xy / u_resolution, 0.0, 1.0);
}
