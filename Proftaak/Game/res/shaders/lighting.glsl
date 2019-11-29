#define PI 3.14159265359
const float W = 1.2;
const float T2 = 7.5;

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

////////////////////////////////////////////////////////////////////////////////
// Cook-Torrance (broken)
////////////////////////////////////////////////////////////////////////////////
vec3 ct_radiance(vec3 n, //Surface normal
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

  vec3 vl = normalize(lightdir);

  return ct_radiance(n, -vl, -ve, roughness, cdiff, cspec, clight);
}

////////////////////////////////////////////////////////////////////////////////
// Lambert
////////////////////////////////////////////////////////////////////////////////
vec3 lambert(vec3 v, vec3 n, vec3 lightdir, vec3 clight, vec3 cdiff) {
  return cdiff * (max(dot(n, lightdir), 0.0) * clight);
}

////////////////////////////////////////////////////////////////////////////////
// Bepis (WIP)
////////////////////////////////////////////////////////////////////////////////
//n: macro normal
//h: half vector
float ggx_distribution(vec3 n, vec3 h, float roughness) {
    float a = roughness*roughness;
    float a2 = a*a;
    float n_dot_h = max(dot(n, h), 0.0);
    float n_dot_h2 = n_dot_h*n_dot_h;

    float num = a2;
    float denom = (n_dot_h2 * (a2 - 1.0) + 1.0);
    denom = PI * denom * denom;

    return num / denom;
}

float geom_schlick_ggx(float n_dot_v, float roughness) {
    // float nom = n_dot_v;
    // float denom = n_dot_v * (1.0 - k) + k;
    // return nom / denom;
    float r = (roughness + 1.0);
    float k = (r*r) / 8.0;

    float num = n_dot_v;
    float denom = n_dot_v * (1.0 - k) + k;
    return num / denom;
}

//n: macro normal
//v: view/ray direction
//l: light direction
//k: roughness
float geom_smith(vec3 n, vec3 v, vec3 l, float roughness) {
    float n_dot_v = max(dot(n, v), 0.0);
    float n_dot_l = max(dot(n, l), 0.0);
    float ggx2 = geom_schlick_ggx(n_dot_v, roughness);
    float ggx1 = geom_schlick_ggx(n_dot_l, roughness);

    return ggx1 * ggx2;
}

vec3 fresnel_schlick(float cos_theta, vec3 f0) {
    cos_theta = min(cos_theta, 1.0); //Needed on older hardware, as apparently a cos_theta above 1 can crash older gpu's lol
    return f0 + (vec3(1.0) - f0) * pow(1.0 - cos_theta, 5.0);
}

//v: view/ray direction
//n: macro normal
vec3 bepis_lighting(vec3 v, vec3 n, vec3 lightdir, vec3 clight, float attenuation, vec3 albedo, float roughness, float metallic) {
    vec3 f0 = vec3(0.04);
    f0 = mix(f0, albedo, metallic);

    vec3 h = normalize(v + lightdir);
    vec3 radiance = clight * attenuation;

    //cook-torrance brdf
    float NDF = ggx_distribution(n, h, roughness);
    float g = geom_smith(n, v, lightdir, roughness);
    vec3 f = fresnel_schlick(max(dot(h, v), 0.0), f0);

    vec3 kD = vec3(1.0) - f;
    kD *= 1.0 - metallic;

    vec3 numerator = NDF * g * f;
    float denominator = 4.0 * max(dot(n, v), 0.0) * max(dot(n, lightdir), 0.0);
    vec3 specular = numerator / max(denominator, 0.001);

    float n_dot_l = max(dot(n, lightdir), 0.0);
    return (kD * albedo / PI + specular) * radiance * n_dot_l;
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

////////////////////////////////////////////////////////////////////////////////
// Reinhard tonemapping
////////////////////////////////////////////////////////////////////////////////
float filmic_reinhard_curve(float x) {
    float q = (T2 * T2 + 1.0) * x * x;
    return q / (q + x + T2*T2);
}

vec3 filmic_reinhard(vec3 x) {
    float w = filmic_reinhard_curve(W);
    return vec3(filmic_reinhard_curve(x.r),
                filmic_reinhard_curve(x.g),
                filmic_reinhard_curve(x.b)) / W;
}

////////////////////////////////////////////////////////////////////////////////
// Hejl2015
////////////////////////////////////////////////////////////////////////////////
vec3 tonemap_filmic_hejl2015(vec3 hdr, float whitePt) {
    vec4 vh = vec4(hdr, whitePt);
    vec4 va = 1.425 * vh + 0.05;
    vec4 vf = (vh * va + 0.004) / (vh * (va + 0.55) + 0.0491) - 0.0821;
    return vf.rgb / vf.www;
}

vec3 shading(vec3 v, vec3 n, vec3 hitpos) {
  //Testing variables
  float roughness = 0.85;
  float metallic = 1.0;
  vec3 albedo = vec3(1.0);
  vec3 cspec = vec3(1.0);

  //Testing light
  vec3 lightdir = normalize(vec3(-0.5, 0.5, -0.5)); //Light direction
  vec3 clight = vec3(23.47, 21.31, 20.79); //Light colour multiplied with intensity
  //NOTE: code for pointlight falloff `falloff = (clamp10(1-(d/lightRadius)^4)^2)/d*d+1`

  float attenuation = softshadow(hitpos, lightdir);

  // return cook_torrance_2(v, n, lightdir, clight, cdiff, cspec, roughness) * attenuation;
  // return lambert(v, n, lightdir, clight, cdiff) * attenuation;
  vec3 final = bepis_lighting(-normalize(v), normalize(n), lightdir, clight, attenuation, albedo, roughness, metallic);
  // final = final / (final + vec3(1.0));
  // final = pow(final, vec3(1.0 / 2.2));
  return filmic_reinhard(final);
  // return pow(tonemap_filmic_hejl2015(final, W), vec3(1.0/2.2));
}
