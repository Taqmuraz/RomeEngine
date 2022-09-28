#version 400 core

in vec3 position;
in vec2 textureCoords;
in vec3 normal;
in vec3 weights;
in vec3 joints;

out vec2 uv;
out vec3 surfaceNormal;

const int MAX_TRANSFORMS = 100;

uniform float time;
uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 jointTransforms[MAX_TRANSFORMS];

void main ()
{
	uv = textureCoords;

	vec3 totalPos = vec3(0);
	vec3 totalNormal = vec3(0);

	for (int i = 0; i < 3; i++)
	{
		int j = int(joints[i]);
		if (j != -1)
		{
			totalPos += (jointTransforms[j] * vec4(position, 1)).xyz * weights[i];
			totalNormal += (jointTransforms[j] * vec4(normal, 0)).xyz * weights[i];
		}
	}
	surfaceNormal = normalize(totalNormal);
	
	gl_Position = projectionMatrix * viewMatrix * vec4(totalPos, 1);
}


