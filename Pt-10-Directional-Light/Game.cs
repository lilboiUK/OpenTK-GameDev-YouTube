using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTK_Game;

public class Game : GameWindow
{
    private readonly List<GameObject> _gameObjects = [];

    private GameObject _cameraGameObject;
    private Camera _camera;
    private GameObject _lightGameObject;
    private Light _light;
    private GameObject _groundGameObject;
    private GameObject _boxGameObject;

    private readonly Vector3[] _boxVertices =
    [
        // Front face
        new(-0.5f, -0.5f, 0.5f),
        new(0.5f, -0.5f, 0.5f),
        new(0.5f, 0.5f, 0.5f),
        new(-0.5f, 0.5f, 0.5f),

        // Back face
        new(-0.5f, -0.5f, -0.5f),
        new(-0.5f, 0.5f, -0.5f),
        new(0.5f, 0.5f, -0.5f),
        new(0.5f, -0.5f, -0.5f),

        // Left face
        new(-0.5f, -0.5f, -0.5f),
        new(-0.5f, -0.5f, 0.5f),
        new(-0.5f, 0.5f, 0.5f),
        new(-0.5f, 0.5f, -0.5f),

        // Right face
        new(0.5f, -0.5f, -0.5f),
        new(0.5f, -0.5f, 0.5f),
        new(0.5f, 0.5f, 0.5f),
        new(0.5f, 0.5f, -0.5f),

        // Top face
        new(-0.5f, 0.5f, -0.5f),
        new(-0.5f, 0.5f, 0.5f),
        new(0.5f, 0.5f, 0.5f),
        new(0.5f, 0.5f, -0.5f),

        // Bottom face
        new(-0.5f, -0.5f, -0.5f),
        new(-0.5f, -0.5f, 0.5f),
        new(0.5f, -0.5f, 0.5f),
        new(0.5f, -0.5f, -0.5f)
    ];

    private readonly Vector3[] _boxNormals =
    [
        // Front face
        new(0.0f, 0.0f, 1.0f),
        new(0.0f, 0.0f, 1.0f),
        new(0.0f, 0.0f, 1.0f),
        new(0.0f, 0.0f, 1.0f),

        // Back face
        new(0.0f, 0.0f, -1.0f),
        new(0.0f, 0.0f, -1.0f),
        new(0.0f, 0.0f, -1.0f),
        new(0.0f, 0.0f, -1.0f),

        // Left face
        new(-1.0f, 0.0f, 0.0f),
        new(-1.0f, 0.0f, 0.0f),
        new(-1.0f, 0.0f, 0.0f),
        new(-1.0f, 0.0f, 0.0f),

        // Right face
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 0.0f, 0.0f),
        new(1.0f, 0.0f, 0.0f),

        // Top face
        new(0.0f, 1.0f, 0.0f),
        new(0.0f, 1.0f, 0.0f),
        new(0.0f, 1.0f, 0.0f),
        new(0.0f, 1.0f, 0.0f),

        // Bottom face
        new(0.0f, -1.0f, 0.0f),
        new(0.0f, -1.0f, 0.0f),
        new(0.0f, -1.0f, 0.0f),
        new(0.0f, -1.0f, 0.0f)
    ];

    private readonly Vector2[] _boxTexCoords =
    [
        // Front face
        new(0.0f, 0.0f),
        new(1.0f, 0.0f),
        new(1.0f, 1.0f),
        new(0.0f, 1.0f),

        // Back face
        new(1.0f, 0.0f),
        new(1.0f, 1.0f),
        new(0.0f, 1.0f),
        new(0.0f, 0.0f),

        // Left face
        new(0.0f, 0.0f),
        new(1.0f, 0.0f),
        new(1.0f, 1.0f),
        new(0.0f, 1.0f),

        // Right face
        new(1.0f, 0.0f),
        new(0.0f, 0.0f),
        new(0.0f, 1.0f),
        new(1.0f, 1.0f),

        // Top face
        new(0.0f, 1.0f),
        new(0.0f, 0.0f),
        new(1.0f, 0.0f),
        new(1.0f, 1.0f),

        // Bottom face
        new(0.0f, 0.0f),
        new(0.0f, 1.0f),
        new(1.0f, 1.0f),
        new(1.0f, 0.0f)
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

    public Game(int width, int height, bool fullscreen, string title) : base(GameWindowSettings.Default,
        NativeWindowSettings.Default)
    {
        Title = title;
        CenterWindow(new Vector2i(width, height));
        if (fullscreen) WindowState = WindowState.Fullscreen;
        CursorState = CursorState.Grabbed;

        Input.Mouse = MouseState;
        Input.Keyboard = KeyboardState;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        var litShader = new Shader("shaders/shader.vert", "shaders/lit.frag");

        var boxMaterial = new Material(litShader);
        boxMaterial.SetProperty("texture0", Texture.LoadFromFile("textures/sandstone-texture.jpg"));

        var groundMaterial = new Material(litShader);
        groundMaterial.SetProperty("texture0", Texture.LoadFromFile("textures/grid-texture.jpg"));
        groundMaterial.SetProperty("tiling", new Vector2(100.0f, 100.0f));

        var boxMesh = new Mesh(_boxVertices, _boxNormals, _boxTexCoords, _boxIndices);

        _cameraGameObject = new GameObject();
        _camera = _cameraGameObject.AddComponent<Camera>();
        _camera.AspectRatio = Size.X / (float)Size.Y;
        _cameraGameObject.AddComponent<CameraController>();
        _cameraGameObject.Transform.Position = new Vector3(0.0f, 2.0f, -5.0f);
        _gameObjects.Add(_cameraGameObject);

        _lightGameObject = new GameObject();
        _light = _lightGameObject.AddComponent<Light>();
        _lightGameObject.Transform.Rotate(-45.0f, 22.5f, 0.0f);
        _gameObjects.Add(_lightGameObject);

        _boxGameObject = new GameObject();
        var boxMeshRenderer = _boxGameObject.AddComponent<MeshRenderer>();
        _boxGameObject.AddComponent<BoxController>();
        boxMeshRenderer.Init(boxMaterial, boxMesh);
        _gameObjects.Add(_boxGameObject);

        _groundGameObject = new GameObject();
        var groundMeshRenderer = _groundGameObject.AddComponent<MeshRenderer>();
        groundMeshRenderer.Init(groundMaterial, boxMesh);
        _groundGameObject.Transform.Position = new Vector3(0.0f, -5.0f, 0.0f);
        _groundGameObject.Transform.Scale = new Vector3(100.0f, 0.1f, 100.0f);
        _gameObjects.Add(_groundGameObject);

        foreach (var gameObject in _gameObjects) gameObject.StartComponents();

        GL.ClearColor(0.529f, 0.808f, 0.922f, 1f);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        Time.DeltaTime = (float)args.Time;

        foreach (var gameObject in _gameObjects) gameObject.UpdateComponents();

        if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Enable(EnableCap.DepthTest);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        foreach (var gameObject in _gameObjects)
            gameObject.GetComponent<MeshRenderer>()?.Render(_camera, _light);

        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}