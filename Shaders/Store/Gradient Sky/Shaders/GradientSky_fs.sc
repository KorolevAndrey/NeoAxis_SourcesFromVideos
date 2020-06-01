$input v_texCoord0

// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
#include "../../../Base/Shaders/Common.sh"

uniform vec4 topColor;
uniform vec4 horizonColor;
uniform vec4 bottomColor;
uniform vec4/*float*/ topExponent;
uniform vec4/*float*/ bottomExponent;
uniform vec4/*vec3*/ multiplier;

void main()
{
	float p = normalize(v_texCoord0).z;
	float p1 = 1.0f - pow(min(1.0f, 1.0f - p), topExponent.x);
	float p3 = 1.0f - pow(min(1.0f, 1.0f + p), bottomExponent.x);
	float p2 = 1.0f - p1 - p3;
	gl_FragColor = vec4((topColor.rgb * p1 + horizonColor.rgb * p2 + bottomColor.rgb * p3) * multiplier.rgb, 1);
}
