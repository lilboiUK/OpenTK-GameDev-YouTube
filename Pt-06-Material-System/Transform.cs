using OpenTK.Mathematics;
namespace OpenTK_Game;

public class Transform(GameObject gameObject) : Component(gameObject)
{
    public Vector3 Position = Vector3.Zero;
    public Quaternion Rotation = Quaternion.Identity;
    public Vector3 Scale = Vector3.One;

    public Matrix4 GetModelMatrix()
    {
        var scale = Matrix4.CreateScale(Scale);
        var rotation = Matrix4.CreateFromQuaternion(Rotation);
        var translation = Matrix4.CreateTranslation(Position);
        
        return translation * rotation * scale;
    }
}