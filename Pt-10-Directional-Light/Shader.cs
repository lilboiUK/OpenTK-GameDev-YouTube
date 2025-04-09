using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_Game;

public class Shader
{
    private readonly int _handle;

    private readonly int _modelUniformLocation;
    private readonly int _viewUniformLocation;
    private readonly int _projectionUniformLocation;

    private readonly int _lightDirUniformLocation;
    private readonly int _lightColorUniformLocation;

    public Shader(string vertexPath, string fragmentPath)
    {
        if (!File.Exists(vertexPath) || !File.Exists(fragmentPath))
            throw new FileNotFoundException("Shader file not found.");

        var vertexShaderSource = File.ReadAllText(vertexPath);
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);
        CheckShaderCompileStatus(vertexShader);

        var fragmentShaderSource = File.ReadAllText(fragmentPath);
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);
        CheckShaderCompileStatus(fragmentShader);

        _handle = GL.CreateProgram();
        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);
        GL.LinkProgram(_handle);
        CheckProgramLinkStatus(_handle);

        GL.DetachShader(_handle, vertexShader);
        GL.DetachShader(_handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        GL.UseProgram(_handle);
        _modelUniformLocation = GL.GetUniformLocation(_handle, "model");
        _viewUniformLocation = GL.GetUniformLocation(_handle, "view");
        _projectionUniformLocation = GL.GetUniformLocation(_handle, "projection");
        _lightDirUniformLocation = GL.GetUniformLocation(_handle, "lightDir");
        _lightColorUniformLocation = GL.GetUniformLocation(_handle, "lightColor");
    }

    private static void CheckShaderCompileStatus(int shader)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var status);
        if (status != (int)All.False) return;
        var infoLog = GL.GetShaderInfoLog(shader);
        throw new Exception($"Shader compilation failed: {infoLog}");
    }

    private static void CheckProgramLinkStatus(int program)
    {
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var status);
        if (status != (int)All.False) return;
        var infoLog = GL.GetProgramInfoLog(program);
        throw new Exception($"Shader program linking failed: {infoLog}");
    }

    public void Use(Matrix4 model, Matrix4 view, Matrix4 projection, Light light)
    {
        GL.UseProgram(_handle);

        GL.UniformMatrix4(_modelUniformLocation, true, ref model);
        GL.UniformMatrix4(_viewUniformLocation, true, ref view);
        GL.UniformMatrix4(_projectionUniformLocation, true, ref projection);
        GL.Uniform3(_lightDirUniformLocation, light.Transform.Forward);
        GL.Uniform3(_lightColorUniformLocation, light.Color);
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

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(_handle, attribName);
    }

    public void Dispose()
    {
        GL.DeleteProgram(_handle);
    }
}