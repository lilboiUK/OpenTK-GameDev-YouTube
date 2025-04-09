#version 330 core
out vec4 FragColor;

uniform vec3 color;
uniform sampler2D texture0;
uniform vec2 tiling;

uniform vec3 lightColor;
uniform vec3 lightDir;

in vec3 Normal;
in vec2 TexCoord;

void main()
{
    float ambientStrength = 0.25;
    vec3 ambient = ambientStrength * lightColor;

    vec3 norm = normalize(Normal);

    float diff = max(dot(norm, -lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    vec3 result = (ambient + diffuse) * color;

    vec2 tiledTexCoord = TexCoord * tiling;
    vec2 repeatedCoord = fract(tiledTexCoord);

    FragColor = vec4(result, 1.0) * texture(texture0, repeatedCoord);
}