#pragma once

struct Material {
	vec3 baseColour;

	float refractiveIndex;
};

uniform Material u_materials[256];