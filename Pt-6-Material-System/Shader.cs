using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_Game;

public class Shader
{
    private readonly int _handle;
    private readonly int _transformUniformLocation;

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
        _handle = GL.CreateProgram();
        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);
        GL.LinkProgram(_handle);
        CheckProgramLinkStatus(_handle);

        // Cleanup
        GL.DetachShader(_handle, vertexShader);
        GL.DetachShader(_handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
        
        GL.UseProgram(_handle);
        _transformUniformLocation = GL.GetUniformLocation(_handle, "transform");
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

    public void Use(Matrix4 modelMatrix)
    {
        GL.UseProgram(_handle);
        GL.UniformMatrix4(_transformUniformLocation, true, ref modelMatrix);
    }

    public int GetUniformsCount()
    {
        GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out var uniformCount);
        return uniformCount;
    }

    public string GetUniformName(int index)
    {
        return GL.GetActiveUniform(_handle, index, out _, out _);
    }

    public ActiveUniformType GetActiveUniformType(int index)
    {
        GL.GetActiveUniform(_handle, index, out _, out var activeUniformType);
        return activeUniformType;
    }

    public int GetUniformLocation(string name)
    {
        return GL.GetUniformLocation(_handle, name);
    }

    public void Dispose()
    {
        GL.DeleteProgram(_handle);
    }
}