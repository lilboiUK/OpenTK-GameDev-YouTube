using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTK_Game;

public class Game : GameWindow
{
    private readonly List<GameObject> _gameObjects = [];
    
    private readonly float[] _boxVertices =
    [
        0.5f, 0.5f, 0.0f, // Top right
        0.5f, -0.5f, 0.0f, // Bottom right
        -0.5f, -0.5f, 0.0f, // Bottom left
        -0.5f, 0.5f, 0.0f // Top left
    ];
    
    private readonly uint[] _boxIndices =
    [
        0, 1, 3,
        1, 2, 3
    ];

    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        CenterWindow(new Vector2i(1000, 1000));
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.2f, 1f, 1f);

        var myShader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        
        var myMaterial = new Material(myShader);
        myMaterial.SetProperty("color", new Vector4(1.0f, 1.0f, 0.0f, 1.0f));
        
        var myGameObject = new GameObject();
        var meshRenderer = myGameObject.AddComponent<MeshRenderer>();
        meshRenderer.Mesh = new Mesh(_boxVertices, _boxIndices);
        meshRenderer.Material = myMaterial;
        _gameObjects.Add(myGameObject);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        foreach (var gameObject in _gameObjects)
            gameObject.GetComponent<MeshRenderer>()?.Render();
        
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}