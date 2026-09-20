#version 450 core

in vec2 vTexCoord;

uniform vec4 uColor;

out vec4 FragColor;

void main()
{
    FragColor = uColor;
}