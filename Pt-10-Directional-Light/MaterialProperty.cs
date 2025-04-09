using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_Game;

public class MaterialProperty
{
    private static readonly Dictionary<Type, object> DefaultValues = new()
    {
        { typeof(Vector2), Vector2.One },
        { typeof(Vector3), Vector3.One },
        { typeof(Matrix4), Matrix4.Identity },
        { typeof(Texture), Texture.DefaultTexture() },
        { typeof(bool), false }
    };

    private object _value;

    public string Name { get; private set; }
    public int ShaderUniformLocation { get; private set; }
    public Type PropertyType { get; }

    public object Value
    {
        get => _value;
        set
        {
            if (value == null || value.GetType() != PropertyType)
                throw new ArgumentException($"MaterialProperty.Value: Value must be of type {PropertyType}");
            _value = value;
        }
    }

    public MaterialProperty(string name, int uniformLocation, ActiveUniformType activeUniformType)
    {
        Name = name;
        ShaderUniformLocation = uniformLocation;
        PropertyType = GetPropertyType(activeUniformType);
        _value = GetDefaultValue();
    }

    private object GetDefaultValue()
    {
        if (DefaultValues.TryGetValue(PropertyType, out var defaultValue))
            return defaultValue;
        throw new NotImplementedException(
            $"MaterialProperty.GetDefaultValue(): Unsupported property type: {PropertyType}");
    }

    private static Type GetPropertyType(ActiveUniformType activeUniformType)
    {
        return activeUniformType switch
        {
            ActiveUniformType.DoubleVec2 => typeof(Vector2),
            ActiveUniformType.FloatVec2 => typeof(Vector2),
            ActiveUniformType.DoubleVec3 => typeof(Vector3),
            ActiveUniformType.FloatVec3 => typeof(Vector3),
            ActiveUniformType.FloatMat4 => typeof(Matrix4),
            ActiveUniformType.Sampler2D => typeof(Texture),
            _ => throw new NotImplementedException(
                $"MaterialProperty.GetPropertyType(): Unsupported ActiveUniformType type: {activeUniformType}")
        };
    }
}