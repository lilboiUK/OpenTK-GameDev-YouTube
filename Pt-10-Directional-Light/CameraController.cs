using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTK_Game;

public class CameraController(GameObject gameObject) : BehaviourScript(gameObject)
{
    private const float MoveSpeed = 5.0f;
    private const float MouseSensitivity = 500.0f;

    public override void Update()
    {
        base.Update();
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        var moveAmount = MoveSpeed * Time.DeltaTime;
        
        if(Input.Keyboard.IsKeyDown(Keys.W)) Transform.Position += Transform.Forward * moveAmount;
        if(Input.Keyboard.IsKeyDown(Keys.S)) Transform.Position -= Transform.Forward * moveAmount;
        if(Input.Keyboard.IsKeyDown(Keys.A)) Transform.Position -= Transform.Right * moveAmount;
        if(Input.Keyboard.IsKeyDown(Keys.D)) Transform.Position += Transform.Right * moveAmount;
        if(Input.Keyboard.IsKeyDown(Keys.Space)) Transform.Position += Transform.Up * moveAmount;
        if(Input.Keyboard.IsKeyDown(Keys.LeftShift)) Transform.Position -= Transform.Up * moveAmount;
    }

    private void HandleRotation()
    {
        var rotationAmount = MouseSensitivity * Time.DeltaTime;

        var deltaX = Input.Mouse.X - Input.Mouse.PreviousX;
        var deltaY = Input.Mouse.Y - Input.Mouse.PreviousY;
        
        Transform.Rotate(Vector3.UnitY, -deltaX * rotationAmount);
        Transform.Rotate(Transform.Right, -deltaY * rotationAmount);
    }
}