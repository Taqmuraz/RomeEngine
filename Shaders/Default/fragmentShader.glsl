#version 400 core

in vec2 pass_textureCoords;
in vec3 surfaceNormal;

out vec4 out_Color;

uniform sampler2D textureSampler;
uniform vec4 textureColor;
uniform vec4 lightColor;
uniform vec3 lightDirection;
uniform float ambienceIntencivity;

void main (void)
{
	vec4 clr = texture(textureSampler, pass_textureCoords);
	
	float nDotl = dot (surfaceNormal, -lightDirection);
	float brightness = max(nDotl, ambienceIntencivity);
	
	out_Color = clr + (lightColor - clr) * brightness;
}


