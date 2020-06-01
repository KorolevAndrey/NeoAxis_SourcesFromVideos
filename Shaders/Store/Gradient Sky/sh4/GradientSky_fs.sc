$input v_texCoord0

// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
#include "../../../Base/Shaders/Common.sh"

uniform vec4/*float*/ topExponent;

void main() {
	
	float time444 = abs(sin(topExponent.x)) * 1.0;
	vec2 position = ( gl_FragCoord.xy / vec2(1000.0,800.0) );
	
	float x = sin(position.x);
	float y = cos(position.y);
	
	gl_FragColor = vec4(vec3(x, y, min(x,y) / time444), 1.0);

}
