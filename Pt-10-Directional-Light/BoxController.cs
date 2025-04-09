using OpenTK.Mathematics;

namespace OpenTK_Game;

public class BoxController(GameObject gameObject) : Component(gameObject)
{
    public override void Update()
    {
        base.Update();
        Transform.Rotate(Vector3.UnitY, 50.0f * Time.DeltaTime);
    }
}