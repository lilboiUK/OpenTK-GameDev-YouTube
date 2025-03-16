using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTK_Game;

public class Game : GameWindow
{
    private readonly uint[] _boxIndices =
    [
        0, 1, 3,
        1, 2, 3
    ];

    private readonly float[] _boxVertices =
    [
        0.5f, 0.5f, 0.0f, // Top right
        0.5f, -0.5f, 0.0f, // Bottom right
        -0.5f, -0.5f, 0.0f, // Bottom left
        -0.5f, 0.5f, 0.0f // Top left
    ];
    
    private readonly List<GameObject> _gameObjects = [];
    
    GameObject _myGameObject;

    private float moveSpeed = 5.0f;

    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        CenterWindow(new Vector2i(1000, 1000));
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.2f, 1f, 1f);

        var myShader = new BasicColorShader("shaders/shader.vert", "shaders/shader.frag")
        {
            Color = Color4.Red
        };
        
        _myGameObject = new GameObject();
        var meshRenderer = _myGameObject.AddComponent<MeshRenderer>();
        meshRenderer.Mesh = new Mesh(_boxVertices, _boxIndices);
        meshRenderer.Shader = myShader;
        _gameObjects.Add(_myGameObject);

        _myGameObject.Transform.Scale *= 0.25f;
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        if (KeyboardState.IsKeyDown(Keys.Escape)) Close();

        int xInput = 0;
        int yInput = 0;
        
        if(KeyboardState.IsKeyDown(Keys.W)) yInput += 1;
        if(KeyboardState.IsKeyDown(Keys.S)) yInput -= 1;
        if(KeyboardState.IsKeyDown(Keys.A)) xInput -= 1;
        if(KeyboardState.IsKeyDown(Keys.D)) xInput += 1;
        
        var velocity = new Vector3(xInput, yInput, 0.0f) * moveSpeed * (float)args.Time;
        
        _myGameObject.Transform.Position += velocity;
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        foreach (var gameObject in _gameObjects)
        {
            gameObject.GetComponent<MeshRenderer>()?.Render();
        }
        
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}