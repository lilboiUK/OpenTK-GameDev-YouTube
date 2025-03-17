using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_Game;

public class Shader
{
    protected readonly int Handle;
    protected readonly int TransformUniformLocation;

    public Shader(string vertexPath, string fragmentPath)
    {
        if (!File.Exists(vertexPath) || !File.Exists(fragmentPath))
            throw new FileNotFoundException("Shader file not found");

        // Vert shader
        var vertexShaderSource = File.ReadAllText(vertexPath);
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);
        CheckShaderCompilation(vertexShader);

        // Frag shader
        var fragmentSource = File.ReadAllText(fragmentPath);
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentSource);
        GL.CompileShader(fragmentShader);
        CheckShaderCompilation(fragmentShader);

        // Link
        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);
        GL.LinkProgram(Handle);
        CheckProgramLinkStatus(Handle);

        // Cleanup
        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
        
        GL.UseProgram(Handle);
        TransformUniformLocation = GL.GetUniformLocation(Handle, "transform");
    }

    private void CheckShaderCompilation(int shader)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var status);
        if (status != (int)All.False) return;
        var infoLog = GL.GetShaderInfoLog(shader);
        throw new Exception($"Shader compilation failed: {infoLog}");
    }

    private void CheckProgramLinkStatus(int program)
    {
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var status);
        if (status != (int)All.False) return;
        var infoLog = GL.GetProgramInfoLog(program);
        throw new Exception($"Shader program linking failed: {infoLog}");
    }

    public virtual void Use(Matrix4 modelMatrix)
    {
        GL.UseProgram(Handle);
        GL.UniformMatrix4(TransformUniformLocation, true, ref modelMatrix);
    }

    public void Dispose()
    {
        GL.DeleteProgram(Handle);
    }
}