using OpenTK.Mathematics;

namespace OpenTK_Game;

public class Transform(GameObject gameObject) : Component(gameObject)
{
    public Vector3 Position { get; set; }= Vector3.Zero;
    public Quaternion Rotation { get; set; }= Quaternion.Identity;
    public Vector3 Scale { get; set; }= Vector3.One;
    
    public Vector3 Forward => Vector3.Transform(-Vector3.UnitZ, Rotation).Normalized();
    public Vector3 Right => Vector3.Transform(Vector3.UnitX, Rotation).Normalized();
    public Vector3 Up => Vector3.Transform(Vector3.UnitY, Rotation).Normalized();

    public Matrix4 GetModelMatrix()
    {
        var scale = Matrix4.CreateScale(Scale);
        var rotation = Matrix4.CreateFromQuaternion(Rotation);
        var translation = Matrix4.CreateTranslation(Position);

        return translation * rotation * scale;
    }

    public void Rotate(Vector3 axis, float angleDegrees)
    {
        var rotation = Quaternion.FromAxisAngle(axis, MathHelper.DegreesToRadians(angleDegrees));
        Rotation = rotation * Rotation;
    }

    public void Rotate(float pitchDegrees, float yawDegrees, float rollDegrees)
    {
        var pitch = Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(pitchDegrees));
        var yaw = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(yawDegrees));
        var roll = Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(rollDegrees));
        Rotation = yaw * pitch * roll * Rotation;
    }

    public void LookAt(Vector3 target, Vector3 up)
    {
        var viewMatrix = Matrix4.LookAt(Position, target, up);
        var rotationMatrix = new Matrix3(viewMatrix);
        Rotation = Quaternion.FromMatrix(rotationMatrix).Inverted();
    }
}