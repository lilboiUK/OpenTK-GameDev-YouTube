using OpenTK.Mathematics;

namespace OpenTK_Game;

public class Camera(GameObject gameObject) : Component(gameObject)
{
    public float AspectRatio = 1.0f;
    public float VerticalFov = 45.0f;
    public float NearClip = 0.1f;
    public float FarClip = 1000.0f;

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.CreateTranslation(Transform.Position);
    }

    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(VerticalFov), AspectRatio, NearClip, FarClip);
    }
}