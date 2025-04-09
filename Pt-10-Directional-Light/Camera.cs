using OpenTK.Mathematics;

namespace OpenTK_Game;

public class Camera(GameObject gameObject) : Component(gameObject)
{
    private float _fov = MathHelper.PiOver2;
    public float AspectRatio { get; set; }

    public float Fov
    {
        get => MathHelper.RadiansToDegrees(_fov);
        set
        {
            var angle = MathHelper.Clamp(value, 1f, 999f);
            _fov = MathHelper.DegreesToRadians(angle);
        }
    }

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Transform.Position, Transform.Position + Transform.Forward, Transform.Up);
    }

    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 1000f);
    }
}