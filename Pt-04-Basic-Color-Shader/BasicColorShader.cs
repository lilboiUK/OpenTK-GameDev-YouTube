using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_Game;

public class BasicColorShader : Shader
{
    public Color4 Color = Color4.White;
    
    public BasicColorShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath) {}

    public override void Use()
    {
        base.Use();

        int vertexColorLocation = GL.GetUniformLocation(Handle, "color");
        GL.Uniform4(vertexColorLocation, Color.R, Color.G, Color.B, Color.A);
    }
}