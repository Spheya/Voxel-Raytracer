struct Ray {
	vec3 origin, direction;
};

struct HitData {
	float dist;
	vec3 normal;
	int material;
};
