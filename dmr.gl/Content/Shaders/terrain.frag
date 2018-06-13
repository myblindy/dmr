#version 450 core

precision highp float;

uniform sampler2D tex;
in vec2 uv_;
out vec4 result;

void main(void) 
{
	result = texture(tex, uv_);
}