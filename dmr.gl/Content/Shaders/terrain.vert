#version 450 core

in  vec2 in_Position;
out vec3 ex_Color;

uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;

void main(void) 
{
    gl_Position = ProjectionMatrix * ViewMatrix * vec4(in_Position, 0.0, 1.0);
    ex_Color = vec3(1.0, 0.0, 1.0);
}