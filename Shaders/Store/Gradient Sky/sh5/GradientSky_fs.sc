$input v_texCoord0

// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
#include "../../../Base/Shaders/Common.sh"

#define SPARKS 30.0
#define FIREWORKS 8.0
#define BASE_PAUSE 8.0/30.0
#define PI2 6.28

uniform vec4/*float*/ topExponent;


float n21(vec2 n) {
    return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}

vec2 randomSpark(float noise) {
    vec2 v0 = vec2((noise - 0.5) * 13.0, (fract(noise * 123.0) - 0.5) * 15.0);
    return v0;
}

vec2 circularSpark(float i, float noiseId, float noiseSpark) {
    float a = (PI2 / float(SPARKS)) * i;
    float speed = 10.0*clamp(noiseId,0.7, 1.0);
    float x = sin(a + topExponent.x*((noiseId-0.5)*3.0));
    float y = cos(a + topExponent.x*(fract(noiseId*4567.332) - 0.5)*2.0);
    vec2 v0 = vec2(x, y) * speed;
    return v0;
}


vec2 rocket(vec2 start, float t) {
    float y = t;
    float x = sin(y*10.0+cos(t*3.0))*0.1;
    vec2 p = start + vec2(x, y * 8.0);
    return p;
}

vec3 firework(vec2 uv, float index, float pauseTime) {
    vec3 col = vec3(0.0,0.0,0.0);


    float timeScale = 1.0;
    vec2 gravity = vec2(0.0, -9.8);

    float explodeTime = 0.9;
    float rocketTime = 1.1;
    float episodeTime = rocketTime + explodeTime + pauseTime;

    float ratio = 1000.0 / 800.0;

    float timeScaled = (topExponent.x - pauseTime) / timeScale;

    float id = floor(timeScaled / episodeTime);
    float et = mod(timeScaled, episodeTime);

    float noiseId = n21(vec2(id+1.0, index+1.0));

    float scale = clamp(fract(noiseId*567.53)*30.0, 10.0, 30.0);
    uv *= scale;

    rocketTime -= (fract(noiseId*1234.543) * 0.5);

    vec2 pRocket = rocket(vec2(0.0 + ((noiseId - 0.5)*scale*ratio), 0.0 - scale/2.0), clamp(et, 0.0, rocketTime));

    if (et < rocketTime) {
        float rd = length(uv - pRocket);
        col += pow(0.05/rd , 1.9) * vec3(0.9, 0.3, 0.0);
    }


    if (et > rocketTime && et < (rocketTime + explodeTime)) {
        float burst = sign(fract(noiseId*44432.22) - 0.6);
        for(int i = 0 ; i < SPARKS ; i++) {
                vec2 center = pRocket;
                float fi = float(i);
                float noiseSpark = fract(n21(vec2(id*10.0+index*20.0, float(i+1))) * 332.44);
                float t = et - rocketTime;
                vec2 v0;

                if (noiseId > 0.5) {
                    v0 = randomSpark(noiseSpark);
                    t -= noiseSpark * (fract(noiseId*543.0) * 0.2);
                } else {
                    v0 = circularSpark(fi, noiseId, noiseSpark);

                    if ( (fract(noiseId*973.22) - 0.5) > 0.0) {
                        float re = mod(fi, 4.0 + 10.0 * noiseId);
                        t -= floor(re/2.0) * burst * 0.1;
                    } else {
                        t -= mod(fi, 2.0) == 0.0 ? 0.0 : burst * 0.5*clamp(noiseId, 0.3, 1.0);
                    }
                }

                vec2 s = v0*t + (gravity * t * t) / 2.0;

                vec2 p = center + s;

                float d = length(uv - p);

                if (t > 0.0) {
                    float fade = (1.0 - t/explodeTime);
                    vec3 sparkColor = vec3(noiseId*0.9, 0.5*fract(noiseId *456.33), 0.5*fract(noiseId *1456.33));
                    vec3 c = (0.05 / d) * sparkColor;
                    col += c * fade;
                }
            }
    }


    return col;
}

vec4 mainImage(vec2 fragCoords) {
    vec2 uv = fragCoords.xy / vec2(1000.0,800.0);
    uv -= 0.5;
    uv.x *= 1000.0 / 800.0;

    vec3 col = vec3(0.0, 0.0, 0.0);

    for (float i = 0.0 ; i < FIREWORKS ; i += 1.0) {
        col += firework(uv, i + 1.0, (i * BASE_PAUSE));
    }


    vec4 fragColor = vec4(col, 1.0);
    return fragColor;
}
// --------[ Original ShaderToy ends here ]---------- //




void main() {
	
    gl_FragColor = mainImage(gl_FragCoord.xy);
}
