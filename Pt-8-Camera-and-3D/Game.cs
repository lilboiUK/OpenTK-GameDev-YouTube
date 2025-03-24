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
        // Position         Texture coordinates
        0.5f, 0.5f, 0.0f, 1.0f, 1.0f, // top right
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        -0.5f, 0.5f, 0.0f, 0.0f, 1.0f // top left
    ];

    private readonly List<GameObject> _gameObjects = [];

    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        CenterWindow(new Vector2i(1000, 1000));
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        var myShader = new Shader("shaders/shader.vert", "shaders/shader.frag");

        var myMaterial = new Material(myShader);
        myMaterial.SetProperty("texture0", Texture.LoadFromFile("textures/stone-texture.png"));
        myMaterial.SetProperty("color", new Vector4(1.0f, 1.0f, 1.0f, 1.0f));

        var myMesh = new Mesh(_boxVertices, _boxIndices);

        var myGameObject = new GameObject();
        var meshRenderer = myGameObject.AddComponent<MeshRenderer>();
        meshRenderer.Init(myMaterial, myMesh);
        _gameObjects.Add(myGameObject);

        GL.ClearColor(0.2f, 0.2f, 1f, 1f);
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