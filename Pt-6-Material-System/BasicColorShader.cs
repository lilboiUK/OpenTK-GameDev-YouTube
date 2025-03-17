using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_Game;

public class BasicColorShader : Shader
{
    private readonly int _colorUniformLocation;
    public Color4 Color = Color4.White;

    public BasicColorShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
    {
        GL.UseProgram(Handle);
        _colorUniformLocation = GL.GetUniformLocation(Handle, "color");
    }

    public override void Use(Matrix4 modelMatrix)
    {
        base.Use(modelMatrix);
        GL.Uniform4(_colorUniformLocation, Color.R, Color.G, Color.B, Color.A);
    }
}