$input v_texCoord0

// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
#include "../../../Base/Shaders/Common.sh"


uniform vec4/*float*/ topExponent;


float gett() {
	return topExponent.x/10.0;
}

float hash(float n) {
	return fract(sin(n)*43758.5435);
}

float noise(vec3 x) {
	vec3 p = floor(x);
	vec3 f = fract(x);
	
	f = f*f*(vec3(3.0,3.0,3.0) - 2.0*f);
	float n = p.x + p.y*57.0 + p.z*113.0;
	
	return mix(
		mix(
			mix(hash(n + 000.0), hash(n + 001.0), f.x),
			mix(hash(n + 057.0), hash(n + 058.0), f.x),
			f.y),
		mix(
			mix(hash(n + 113.0), hash(n + 114.0), f.x),
			mix(hash(n + 170.0), hash(n + 171.0), f.x),
			f.y),
		f.z);
}

float fbm111(vec3 p) {
	float f = 0.0;
	
	f += 0.5000*noise(p); p *= 2.02;
	f += 0.2500*noise(p); p *= 2.04;
	f += 0.1250*noise(p); p *= 2.00;
	f += 0.0625*noise(p); p *= 1.97;
	f /= 0.9375;
	
	return f;
}

float volume(vec3 p, out float rawDens) {
	float dens = -p.y - 1.0;
	
	if (dens < -1.0) return 0.0;
	rawDens = dens + 1.0*fbm111(2.0*p + vec3(gett(),gett(),gett()));
	dens = clamp(rawDens, 0.0, 1.0);
	
	return dens;
}

vec4 volumetric(vec3 ro, vec3 rd, vec3 col, float mt) {
	vec4 sum = vec4(0.0, 0.0, 0.0, 0.0);
	
	float ste = 0.075;
	
	vec3 pos = ro + rd*ste;
	
	for(int i = 0; i < 100; i++) {
		if(sum.a > 0.99) continue;
		
		float dens, rawDens;
		dens = volume(pos, rawDens);
		
		vec4 col2 = vec4(mix(vec3(0.2,0.2,0.2), vec3(1.0,1.0,1.0), dens), dens);
		col2.rgb *= col2.a;
		col2.rgb = mix(col, col2.rgb, clamp(smoothstep(ste - 4.0, ste + 2.8 + ste*0.75, mt), 0.0, 1.0));
		sum = sum + col2*(1.0 - sum.a);
		
		float sm = 1.0 + 2.5*(1.0 - clamp(rawDens+1.0, 0.0, 1.0));
		//if(ste*sm < mt) break;
		pos += rd*ste*sm;
	}
	
	return clamp(sum, 0.0, 1.0);
}


float map(vec3 p) {
	p.y -= 0.2*cos(gett());
	float s =  length(p) - 1.0;

	return s;
}


float march(vec3 ro, vec3 rd) {
	float t = 0.0;
	for(int i = 0; i < 100; i++) {
		float h = map(ro + rd*t);
		if(h < 0.001 || t >= 10.0) break;
		t += h;
	}
	
	return t;
}

vec3 normal(vec3 p) {
	vec2 h = vec2(0.001, 0.0);
	vec3 n = vec3(
		map(p + h.xyy) - map(p - h.xyy),
		map(p + h.yxy) - map(p - h.yxy),
		map(p + h.yyx) - map(p - h.yyx)
	);
	
	return normalize(n);
}

mat3 camera(vec3 eye, vec3 lat) {
	vec3 ww = normalize(lat - eye);
	vec3 uu = normalize(cross(vec3(0.0, 1.0, 0.0), ww));
	vec3 vv = normalize(cross(ww, uu));
		
	return mat3(uu, vv, ww);	
}

void main() {
	vec2 resolution = vec2(1300.0, 600.0);
	vec2 uv = vec2(1.0,1.0) - 2.0*(gl_FragCoord.xy/resolution);
	uv.x *= resolution.x/resolution.y;
	
	vec3 col = vec3(0.0, 0.0, 0.0);
	
	vec3 ro = vec3(1.0,0.0,0.5);//3.0*vec3(cos(0.3*time), 0.0, -sin(0.3*time));

        mat3 matmat = camera(ro, vec3(0.0, 0.0, 0.0));
        vec3 vvv = vec3(uv, 1.97);

        vec3 uuu = vec3(1.0, 1.0, 1.0);
        uuu.x = dot(vvv, matmat[0]); // m[0] is the left column of m
        uuu.y = dot(vvv, matmat[1]); // dot(a,b) is the inner (dot) product of a and b
        uuu.z = dot(vvv, matmat[2]);
        
	vec3 rd = normalize(uuu); //vec3(0.0, 0.0, 0.0); //(matmat * vvv) / 5.0;
	
	float i = march(ro, rd);
	
	/*if(i < 10.0) {
		vec3 pos = ro + rd*i;
		vec3 nor = normal(pos);
		
		col = (0.5 + 0.5*nor.y)*vec3(1.00, 0.97, 0.1);
	}*/
	 
	vec4 fog = volumetric(ro, rd, col, i);
	col = fog.xyz;
	
	col = pow(col, vec3(0.454545, 0.454545, 0.454545));
	
	gl_FragColor = vec4(col, 1.0);
}
