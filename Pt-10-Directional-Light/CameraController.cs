using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTK_Game;

public class CameraController(GameObject gameObject) : BehaviourScript(gameObject)
{
    private const float MoveSpeed = 5.0f;
    private const float MouseSensitivity = 500.0f;

    private float _pitch;
    private float _yaw;

    public override void Update()
    {
        base.Update();
        HandleRotation();
        HandleMovement();
    }

    private void HandleMovement()
    {
        var moveAmount = MoveSpeed * Time.DeltaTime;

        if (Input.Keyboard.IsKeyDown(Keys.W)) Transform.Position += Transform.Forward * moveAmount;
        if (Input.Keyboard.IsKeyDown(Keys.S)) Transform.Position -= Transform.Forward * moveAmount;
        if (Input.Keyboard.IsKeyDown(Keys.A)) Transform.Position -= Transform.Right * moveAmount;
        if (Input.Keyboard.IsKeyDown(Keys.D)) Transform.Position += Transform.Right * moveAmount;
        if (Input.Keyboard.IsKeyDown(Keys.Space)) Transform.Position += Transform.Up * moveAmount;
        if (Input.Keyboard.IsKeyDown(Keys.LeftShift)) Transform.Position -= Transform.Up * moveAmount;
    }

    private void HandleRotation()
    {
        var deltaX = Input.Mouse.Delta.X;
        var deltaY = Input.Mouse.Delta.Y;

        var rotationAmount = MouseSensitivity * Time.DeltaTime;

        _yaw += deltaX * rotationAmount;
        _pitch -= deltaY * rotationAmount;
        _pitch = MathHelper.Clamp(_pitch, -89.0f, 89.0f);

        var pitchRadians = MathHelper.DegreesToRadians(_pitch);
        var yawRadians = MathHelper.DegreesToRadians(_yaw);

        var forward = new Vector3(
            (float)Math.Cos(pitchRadians) * (float)Math.Cos(yawRadians),
            (float)Math.Sin(pitchRadians),
            (float)Math.Cos(pitchRadians) * (float)Math.Sin(yawRadians)
        );
        forward = Vector3.Normalize(forward);

        var right = Vector3.Normalize(Vector3.Cross(forward, Vector3.UnitY));
        var up = Vector3.Cross(right, forward);

        var rotationMatrix = new Matrix3(
            right.X, up.X, -forward.X,
            right.Y, up.Y, -forward.Y,
            right.Z, up.Z, -forward.Z
        );
        
        Transform.Rotation = Quaternion.FromMatrix(rotationMatrix);
    }
}