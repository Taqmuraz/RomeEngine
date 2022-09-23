#version 400 core

in vec3 position;
in vec2 textureCoords;
in vec3 normal;

out vec2 pass_textureCoords;
out vec3 surfaceNormal;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

void main ()
{
	pass_textureCoords = textureCoords;
	
	mat4 vt = viewMatrix * transformationMatrix;
	vec4 world = vt * vec4(position, 1.0);
	vec4 pos = projectionMatrix * world;
	
	vec4 surfaceNormal4 = vt * vec4(normal.x, normal.y, normal.z, 0.0);
	surfaceNormal = normalize(vec3(surfaceNormal4.x, surfaceNormal4.y, surfaceNormal4.z));
	
	gl_Position = pos;
}


