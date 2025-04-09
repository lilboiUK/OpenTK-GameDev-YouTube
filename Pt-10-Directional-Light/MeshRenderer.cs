using OpenTK.Graphics.OpenGL4;

namespace OpenTK_Game;

public class MeshRenderer(GameObject gameObject) : Component(gameObject)
{
    private int _elementBufferObject;
    private Material? _material;
    private Mesh? _mesh;
    private int _vertexArrayObject;
    private int _vertexBufferObject;

    public void Init(Material material, Mesh mesh)
    {
        _material = material;
        _mesh = mesh;

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _mesh.Vertices.Length * sizeof(float), _mesh.Vertices,
            BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _mesh.Indices.Length * sizeof(uint), _mesh.Indices,
            BufferUsageHint.StaticDraw);

        var vertexLocation = _material.Shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        var texCoordLocation = _material.Shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float),
            3 * sizeof(float));

        GL.BindVertexArray(0);
    }

    public void Render(Camera camera)
    {
        if (_material == null || _mesh == null) return;

        var model = gameObject.Transform.GetModelMatrix();
        var view = camera.GetViewMatrix();
        var projection = camera.GetProjectionMatrix();

        _material.Apply(model * view * projection);
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
    }
}