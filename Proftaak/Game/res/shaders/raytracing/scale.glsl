#version 450

uniform sampler2D u_framebuffer;
uniform vec2 u_resolution;

out vec4 colour;

void main() {
  ivec2 bufSize = textureSize(u_framebuffer, 0);
  
  vec2 pixelSize = u_resolution / bufSize;
  vec2 uvPixel = pixelSize / u_resolution;

  vec2 uvCoords = gl_FragCoord.xy / u_resolution;

  colour = texture(u_framebuffer, uvCoords);
   
  
  if(mod(uvCoords, uvPixel).x > 0.0009) {
	colour = vec4(0.0);
  }

  //colour = vec4(vec2(bufSize), 0.0, 1.0);
}
