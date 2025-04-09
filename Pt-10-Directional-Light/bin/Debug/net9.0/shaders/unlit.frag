#version 330 core

in vec2 texCoord;

uniform vec3 color;
uniform sampler2D texture0;
uniform vec2 tiling;

out vec4 FragColor;

void main()
{
    vec2 tiledTexCoord = texCoord * tiling;
    vec2 repeatedCoord = fract(tiledTexCoord);
    FragColor = texture(texture0, repeatedCoord) * vec4(color, 1.0);
}