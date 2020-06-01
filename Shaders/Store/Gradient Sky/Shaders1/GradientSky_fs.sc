$input v_texCoord0

// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
#include "../../../Base/Shaders/Common.sh"

uniform float time;
uniform vec4 topColor;
uniform vec4 horizonColor;
uniform vec4 bottomColor;
uniform vec4/*float*/ topExponent;
uniform vec4/*float*/ bottomExponent;
uniform vec4/*vec3*/ multiplier;


float random123 (in vec2 st) {
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))
                 * 43758.5453123);
}


float random321 (in vec2 st) {
    return fract(sin(dot(st.xy,
                         vec2(12.9898,18.233)))
                 * 13758.5453123);
}


float noise321( in vec3 x )
{
    vec3 p = floor(x);
    vec3 f = fract(x);
	f = f*f*(3.0-2.0*f);
    
	vec2 uv = (p.xy+vec2(37.0,239.0)*p.z) + f.xy;
    //vec2 rg = textureLod(iChannel0,(uv+0.5)/256.0,0.0).yx;
    
	return -1.0+2.0*mix( random123(uv), random321(uv), f.z );
}

float map5( in vec3 p )
{
	vec3 q = p - vec3(0.0,0.1,1.0)*time;
	float f;
    f  = 0.50000*noise321( q ); q = q*2.02;
    f += 0.25000*noise321( q ); q = q*2.03;
    f += 0.12500*noise321( q ); q = q*2.01;
    f += 0.06250*noise321( q ); q = q*2.02;
    f += 0.03125*noise321( q );
	return clamp( 1.5 - p.y - 2.0 + 1.75*f, 0.0, 1.0 );
}
float map4( in vec3 p )
{
	vec3 q = p - vec3(0.0,0.1,1.0)*time;
	float f;
    f  = 0.50000*noise321( q ); q = q*2.02;
    f += 0.25000*noise321( q ); q = q*2.03;
    f += 0.12500*noise321( q ); q = q*2.01;
    f += 0.06250*noise321( q );
	return clamp( 1.5 - p.y - 2.0 + 1.75*f, 0.0, 1.0 );
}
float map3( in vec3 p )
{
	vec3 q = p - vec3(0.0,0.1,1.0)*time;
	float f;
    f  = 0.50000*noise321( q ); q = q*2.02;
    f += 0.25000*noise321( q ); q = q*2.03;
    f += 0.12500*noise321( q );
	return clamp( 1.5 - p.y - 2.0 + 1.75*f, 0.0, 1.0 );
}
float map2( in vec3 p )
{
	vec3 q = p - vec3(0.0,0.1,1.0)*time;
	float f;
    f  = 0.50000*noise321( q ); q = q*2.02;
    f += 0.25000*noise321( q );
	return clamp( 1.5 - p.y - 2.0 + 1.75*f, 0.0, 1.0 );
}


//vec4 integrate( in vec4 sum, in float dif, in float den, in vec3 bgcol, in float t )
//{
    // lighting
//    vec3 lin = vec3(0.65,0.7,0.75)*1.4 + vec3(1.0, 0.6, 0.3)*dif;        
//    vec4 col = vec4( mix( vec3(1.0,0.95,0.8), vec3(0.25,0.3,0.35), den ), den );
//    return sum;
//}

#define MARCH(STEPS,MAPLOD)\
for(int i=0; i<STEPS; i++)\
{\
   vec3 pos = ro + t*rd;\
   if( pos.y<-3.0 || pos.y>2.0 || sum.a>0.99 ) break;\
   float den = MAPLOD( pos );\
   if( den>0.01 )\
   {\
     float dif = clamp((den)/0.6, 0.0, 1.0 );\
     vec3  lin = vec3(0.65,0.7,0.75)*1.4 + vec3(1.0,0.6,0.3)*dif;\
     vec4  col = vec4( mix( vec3(1.0,0.95,0.8), vec3(0.25,0.3,0.35), den ), den );\
     col.xyz *= lin;\
     col.xyz = mix( col.xyz, bgcol, 1.0-exp(-0.003*t*t) );\
     col.w *= 0.4;\
     \
     col.rgb *= col.a;\
     sum += col*(1.0-sum.a);\
   }\
   t += max(0.05,0.02*t);\
}

vec4 raymarch123( in vec3 ro, in vec3 rd, in vec3 bgcol, in ivec2 px )
{
	vec4 sum = vec4(0.0);

	float t = 0.0;

    MARCH(30,map5);
    MARCH(30,map4);
    MARCH(30,map3);
    MARCH(30,map2);

    return clamp( sum, 0.0, 1.0 );
}

mat3 setCamera123( in vec3 ro, in vec3 ta, float cr )
{
	vec3 cw = normalize(ta-ro);
	vec3 cp = vec3(sin(cr), cos(cr),0.0);
	vec3 cu = normalize( cross(cw,cp) );
	vec3 cv = normalize( cross(cu,cw) );
    return mat3( cu, cv, cw );
}

vec4 render123( in vec3 ro, in vec3 rd, in ivec2 px )
{
	vec3 col = vec3(0.6,0.71,0.75);
    vec4 res = raymarch123( ro, rd, col, px );
    col = col*(1.0-res.w) + res.xyz;
    
    return vec4( col, 1.0 );
}

void main()
{
    vec2 aa = vec2(512.0,288.0);
    float bb = 288.0;
    vec2 p = (2.0*v_texCoord0-aa)/bb;

    //vec2 m = iMouse.xy/iResolution.xy;
    
    // camera
    vec3 ro = 4.0*normalize(vec3(sin(3.0*10.0), 0.4, cos(3.0)));
	vec3 ta = vec3(0.0, -1.0, 0.0);
    mat3 ca = setCamera123( ro, ta, 0.0 );
    // ray
    vec3 rd = ca * normalize( vec3(p.xy,1.5));
    
    gl_FragColor = render123( ro, rd, ivec2(v_texCoord0-0.5) );
}

