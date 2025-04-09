using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTK_Game;

public class Game : GameWindow
{
    private readonly List<GameObject> _gameObjects = [];

    private GameObject _myGameObject;
    private GameObject _cameraGameObject;
    private Camera _camera;

    private readonly float[] _boxVertices =
    [
        // Front face
        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 1.0f,
        // Back face
        -0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
        -0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
        0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
        0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
        // Left face
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
        -0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
        -0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
        // Right face
        0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 1.0f,
        0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
        // Top face
        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
        0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
        // Bottom face
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
        -0.5f, -0.5f, 0.5f, 0.0f, 1.0f,
        0.5f, -0.5f, 0.5f, 1.0f, 1.0f,
        0.5f, -0.5f, -0.5f, 1.0f, 0.0f
    ];

    private readonly uint[] _boxIndices =
    [
        0, 1, 2, 2, 3, 0, // Front face
        4, 5, 6, 6, 7, 4, // Back face
        8, 9, 10, 10, 11, 8, // Left face
        12, 13, 14, 14, 15, 12, // Right face
        16, 17, 18, 18, 19, 16, // Top face
        20, 21, 22, 22, 23, 20 // Bottom face
    ];

    public Game(int width, int height, string title) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Title = title;
        CenterWindow(new Vector2i(width, height));
        
        CursorState = CursorState.Hidden;

        Input.Mouse = MouseState;
        Input.Keyboard = KeyboardState;
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        _cameraGameObject = new GameObject();
        _camera = _cameraGameObject.AddComponent<Camera>();
        _camera.AspectRatio = Size.X / (float)Size.Y;
        _cameraGameObject.AddComponent<CameraController>();
        _cameraGameObject.Transform.Position = new Vector3(0.0f, 0.0f, 0.0f);
        _gameObjects.Add(_cameraGameObject);

        var myShader = new Shader("shaders/shader.vert", "shaders/shader.frag");

        var myMaterial = new Material(myShader);
        myMaterial.SetProperty("texture0", Texture.LoadFromFile("textures/sandstone-texture.jpg"));
        myMaterial.SetProperty("color", new Vector4(1.0f, 1.0f, 1.0f, 1.0f));

        var myMesh = new Mesh(_boxVertices, _boxIndices);

        _myGameObject = new GameObject();
        var meshRenderer = _myGameObject.AddComponent<MeshRenderer>();
        meshRenderer.Init(myMaterial, myMesh);
        _gameObjects.Add(_myGameObject);

        GL.ClearColor(0.2f, 0.2f, 1f, 1f);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        Time.DeltaTime = (float)args.Time;
        
        foreach(var component in _gameObjects.Select(gameObject => gameObject.GetComponents<Component>())
                    .SelectMany(components => components)) 
            component.Update();
        
        
        if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Enable(EnableCap.DepthTest);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        foreach (var gameObject in _gameObjects)
            gameObject.GetComponent<MeshRenderer>()?.Render(_camera);

        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}