#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

uniform mat4 modelViewProjectionMatrix;

out vec2 texCoord;

void main()
{
    texCoord = aTexCoord;
    gl_Position = vec4(aPosition, 1.0) * modelViewProjectionMatrix;
}