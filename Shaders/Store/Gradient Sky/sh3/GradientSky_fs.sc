$input v_texCoord0

// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
#include "../../../Base/Shaders/Common.sh"

uniform vec4 topExponent123;


// Necip's transfer from this orginal: https://www.shadertoy.com/view/XsfSR8


float hash555( float n ) { return fract(sin(n)*753.5453123); }

// Slight modification of iq's noise function.
float noise555( in vec2 x )
{
    vec2 p = floor(x);
    vec2 f = fract(x);
    f = f*f*(3.0-2.0*f);
    
    float n = p.x + p.y*157.0;
    return mix(
                    mix( hash555(n+  0.0), hash555(n+  1.0),f.x),
                    mix( hash555(n+157.0), hash555(n+158.0),f.x),
            f.y);
}


float fbm555(vec2 p, vec3 a)
{
     float v = 0.0;
     v += noise555(p*a.x)*.50;
     v += noise555(p*a.y)*.50;
     v += noise555(p*a.z)*.125;
     return v;
}

vec3/*float*/ drawLines555( vec2 uv, vec3 fbmOffset, vec3 color1, vec3 color2, float time111 )
{
    float timeVal = time111 * 0.1;
    vec3 finalColor = vec3( 0.0, 0.0, 0.0 );
    
    for( int i=0; i < 5; ++i )
    {
        float indexAsFloat = float(i);
        float amp = 40.0 + (indexAsFloat*7.0);
        float period = 2.0 + (indexAsFloat+8.0);
        float thickness = mix( 0.7, 1.0, noise555(uv*10.0) );
        float t = abs( 0.8 / (sin(uv.x + fbm555( uv + timeVal * period, fbmOffset )) * amp) * thickness );
        
        finalColor +=  t * color2 * 0.6;
    }
    
    return finalColor;
}


void main() 
{
    vec2 resolution = vec2(1000.0,800.0);

                           //gl_FragCoord
    vec2 uv = ( gl_FragCoord.xy / resolution.xy ) * 2.0 - 1.5;
    uv.x *= resolution.x/resolution.y;
    uv.xy = uv.yx;

    vec3 lineColor1 = vec3( 2.3, 1.5, 0.5 );
    vec3 lineColor2 = vec3( 1.3, 0.5, 2.5 );
    
    vec3 finalColor = vec3(0.0,0.0,0.0);

    float t = sin( topExponent123.x ) * 0.5 + 0.5;
    float pulse = mix( 0.10, 0.20, t);
    
    //finalColor += drawLines( uv, vec3( 1.0, 20.0, 30.0), lineColor1, lineColor2 ) * pulse;
    finalColor += drawLines555( uv, vec3( 1.0, 2.0, 4.0), lineColor1, lineColor2, topExponent123.x );
    
    gl_FragColor = vec4( finalColor, 1.0 );

}
