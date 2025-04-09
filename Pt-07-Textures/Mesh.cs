namespace OpenTK_Game;

public class Mesh(float[] vertices, uint[] indices)
{
    public float[] Vertices { get; private set; } = vertices;
    public uint[] Indices { get; private set; } = indices;
}