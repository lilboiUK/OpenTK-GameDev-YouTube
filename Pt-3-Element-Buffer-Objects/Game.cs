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

    private readonly Shader _shader;
    private int _vertexBufferObject, _vertexArrayObject, _elementBufferObject;

    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        CenterWindow(new Vector2i(1000, 1000));
        _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.2f, 1f, 1f);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _boxVertices.Length * sizeof(float), _boxVertices,
            BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _boxIndices.Length * sizeof(uint), _boxIndices,
            BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindVertexArray(0);
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

        _shader.Use();
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _boxIndices.Length, DrawElementsType.UnsignedInt, 0);
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.DeleteBuffer(_vertexBufferObject);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.DeleteBuffer(_elementBufferObject);
        GL.BindVertexArray(0);
        GL.DeleteVertexArray(_vertexArrayObject);
        _shader.Dispose();
    }
}