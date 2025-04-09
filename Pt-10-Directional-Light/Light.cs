using OpenTK.Mathematics;

namespace OpenTK_Game;

public class Light(GameObject gameObject) : Component(gameObject)
{
    public Vector3 Color { get; set; } = Vector3.One;
}