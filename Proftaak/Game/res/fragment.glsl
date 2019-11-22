#version 400

uniform mat4 u_cameraTransformation;

uniform float u_zoom; // = (screenHeight * 0.5) / tan(fov * 0.5)

struct Ray {
	vec3 origin, direction;
};

Ray generateRay() {
	return Ray(vec3(0,0,0), normalize(vec3(gl_FragCoord.xy, u_zoom)));
}

void main ()
{
	// Generate a local ray and transform it to world space
	Ray ray = generateRay();
	ray.origin = (u_cameraTransformation * vec4(ray.origin, 1.0)).xyz;
	ray.direction = (u_cameraTransformation * vec4(ray.direction, 0.0)).xyz;



	gl_FragColor = vec4(0.0, 1.0, 0.0, 1.0);
}