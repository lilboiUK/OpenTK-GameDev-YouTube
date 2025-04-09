using OpenTK.Mathematics;

namespace OpenTK_Game;

public class Mesh(Vector3[] vertices, Vector3[] normals, Vector2[] texCoords, uint[] indices)
{
    public Vector3[] Vertices { get; private set; } = vertices;
    public Vector3[] Normals { get; private set; } = normals;
    public Vector2[] TexCoords { get; private set; } = texCoords;
    public uint[] Indices { get; private set; } = indices;
}