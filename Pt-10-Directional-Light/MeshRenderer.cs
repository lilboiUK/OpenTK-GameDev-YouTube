using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace OpenTK_Game;

public class MeshRenderer(GameObject gameObject) : Component(gameObject)
{
    private Material? _material;
    private Mesh? _mesh;
    
    private int _vertexArrayObject;
    private int _vertexBufferObject;
    private int _normalBufferObject;
    private int _texCoordBufferObject;
    private int _elementBufferObject;

    public void Init(Material material, Mesh mesh)
    {
        _material = material;
        _mesh = mesh;

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _mesh.Vertices.Length * Vector3.SizeInBytes, _mesh.Vertices,
            BufferUsageHint.StaticDraw);
        
        var vertexLocation = _material.Shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);

        _normalBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _mesh.Normals.Length * Vector3.SizeInBytes, _mesh.Normals,
            BufferUsageHint.StaticDraw);
        
        var normalLocation = _material.Shader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
        
        _texCoordBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _mesh.TexCoords.Length * Vector2.SizeInBytes, _mesh.TexCoords,
            BufferUsageHint.StaticDraw);
        
        var texCoordLocation = _material.Shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);
        
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _mesh.Indices.Length * sizeof(uint), _mesh.Indices,
            BufferUsageHint.StaticDraw);
        
        GL.BindVertexArray(0);
    }

    public void Render(Camera camera, Light light)
    {
        if (_material == null || _mesh == null) return;

        var model = gameObject.Transform.GetModelMatrix();
        var view = camera.GetViewMatrix();
        var projection = camera.GetProjectionMatrix();

        _material.Apply(model, view, projection, light);
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
    }
}