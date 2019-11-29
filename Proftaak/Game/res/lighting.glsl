struct DirectionalLight {
  vec3 direction;

  float intensity;
  vec3 colour;
};

struct PointLight {
  vec3 position;
  vec3 direction;

  float intensity;
  vec3 colour;
  // float fallOff;
};

vec3 radiance(vec3 n, //Surface normal
              vec3 l, //Light direction
              vec3 v, //View/ray direction

              float m, //Roughness
              vec3 cdiff, //Diffuse reflectance
              vec3 cspec, //Specular reflectance

              vec3 clight //Light intensity (RGB)
) {
  vec3 h = normalize(l + v); //Half vector

  float dot_n_h = max( abs( dot( n, h ) ), 0.001 );
  float dot_n_v = max( abs( dot( n, v ) ), 0.001 );
  float dot_n_l = max( abs( dot( n, l ) ), 0.001 );
  float dot_h_v = max( abs( dot( h, v ) ), 0.001 ); //dot_h_v == dot_h_l

  //Cook-Torrance geometric term
  float g = 2.0 * dot_n_h / dot_h_v;
  float G = min( min( dot_n_v, dot_n_l ) * g, 1.0);

  //Normal distribution function
  //Beckmann's method
  float sq_nh = dot_n_h * dot_n_h;
  float sq_nh_m = sq_nh * (m * m);
  float D = exp( ( sq_nh - 1.0 ) / sq_nh_m ) / ( sq_nh * sq_nh_m );

  //Specular Fresnel term (Schlick's approximation)
  vec3 Fspec = cspec + (1.0 - cspec) * pow(1.0 - dot_h_v, 5.0);

  //Diffuse Fresnel term (still Schlick's lol)
  vec3 Fdiff = cspec + (1.0 - cspec) * pow(1.0 - dot_n_l, 5.0);

  //Cook-Torrance BRDF
  vec3 brdf_spec = Fspec * D * G / (dot_n_v * dot_n_l * 4.0);

  //Lambertian BRDF
  vec3 brdf_diff = cdiff * (1.0 - Fdiff);

  //Punctual light source
  return (brdf_spec + brdf_diff) * clight * dot_n_l;
}

//v: view direction
//n: normal direction
vec3 cook_torrance(vec3 v, vec3 n, vec3 lightdir, vec3 clight, vec3 cdiff, vec3 cspec, float roughness) {
  vec3 ve = normalize(v);

  vec3 final = vec3(0.0);

  vec3 vl = normalize(lightdir);

  final += radiance(n, -vl, -ve, roughness, cdiff, cspec, clight);

  return final;
}

vec3 lambert(vec3 v, vec3 n, vec3 lightdir, vec3 clight, vec3 cdiff) {
  return cdiff * (max(dot(n, lightdir), 0.0) * clight);
}

float softshadow(vec3 ro, vec3 rd) {
  float res = 1.0;
  Ray ray = Ray(ro + rd * 0.002, rd);

  HitData hit = trace(ray);
  if (hit.dist <  WORLD_RENDER_DISTANCE) {
    return clamp(hit.dist * 0.002, 0.0, 1.0);
    // return 0.0;
  } else {
    return 1.0;
  }
}

vec3 shading(vec3 v, vec3 n, vec3 hitpos) {
  //Testing variables
  float roughness = 0.85;
  vec3 cdiff = vec3(1.0);
  vec3 cspec = vec3(1.0);

  //Testing light
  vec3 lightdir = vec3(-0.5, 0.5, -0.5); //Light direction
  vec3 clight = vec3(0.4, 0.6, 0.8); //Light colour multiplied with intensity

  // return cook_torrance(v, n, lightdir, clight, cdiff, cspec, roughness);
  return lambert(v, n, lightdir, clight, cdiff) * softshadow(hitpos, lightdir);
}
