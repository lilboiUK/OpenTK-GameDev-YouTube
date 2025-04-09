using OpenTK.Graphics.OpenGL4;

namespace OpenTK_Game;

public class MeshRenderer(GameObject gameObject) : Component(gameObject)
{
    private Mesh? _mesh;
    
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    private int _elementBufferObject;
    
    public Shader? Shader;

    public Mesh? Mesh
    {
        get => _mesh;
        set
        {
            _mesh = value;
            InitMesh();
        }
    }

    private void InitMesh()
    {
        if(_mesh == null) return;
        
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _mesh.Vertices.Length * sizeof(float), _mesh.Vertices, BufferUsageHint.StaticDraw);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _mesh.Indices.Length * sizeof(uint), _mesh.Indices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindVertexArray(0);
    }

    public void Render()
    {
        if(Shader == null || _mesh == null) return;
        
        Shader.Use(GameObject.Transform.GetModelMatrix());
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
    }
}