using OpenTK.Graphics.ES30;

namespace OpenTK_Game;

public class Shader
{
    private readonly int _handle;

    public Shader(string vertPath, string fragPath)
    {
        // Vert shader
        string vertSource = File.ReadAllText(vertPath);
        int vertShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertShader, vertSource);
        GL.CompileShader(vertShader);
        
        // Frag shader
        string fragSource = File.ReadAllText(fragPath);
        int fragShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragShader, fragSource);
        GL.CompileShader(fragShader);
        
        // Link
        _handle = GL.CreateProgram();
        GL.AttachShader(_handle, vertShader);
        GL.AttachShader(_handle, fragShader);
        GL.LinkProgram(_handle);

        // Cleanup
        GL.DetachShader(_handle, vertShader);
        GL.DetachShader(_handle, fragShader);
        GL.DeleteShader(vertShader);
        GL.DeleteShader(fragShader);
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }

    public void Dispose()
    {
        GL.DeleteProgram(_handle);
    }
}