#version 330 core

out vec4 fragColor;

uniform vec4 color;
uniform vec4 color2;

void main()
{
    fragColor = color2;
}