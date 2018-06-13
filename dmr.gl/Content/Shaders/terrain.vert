#version 450 core

in  vec2 vert;
in  vec2 uv;
out vec2 uv_;

uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;

void main(void) 
{
    gl_Position = ProjectionMatrix * ViewMatrix * vec4(vert, 0.0, 1.0);
	uv_ = uv;
}