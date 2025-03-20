using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTK_Game;

public class Material
{
    private readonly List<MaterialProperty> _materialProperties = [];
    public Shader Shader { get; private set; }

    public Material(Shader shader)
    {
        Shader = shader;
        UpdatePropertiesList();
    }
    
    public void SetShader(Shader shader)
    {
        Shader = shader;
        UpdatePropertiesList();
    }

    public void SetProperty(string propertyName, object value)
    {
        foreach (var property in _materialProperties.Where(property => property.Name == propertyName))
            property.Value = value;
    }

    private void UpdatePropertiesList()
    {
        _materialProperties.Clear();
        for (var i = 0; i < Shader.GetUniformsCount(); i++)
        {
            var name = Shader.GetUniformName(i);
            if (name == "transform") continue; // todo Find better solution
            var location = Shader.GetUniformLocation(name);
            var activeUniformType = Shader.GetActiveUniformType(i);
            var materialProperty = new MaterialProperty(name, location, activeUniformType);
            _materialProperties.Add(materialProperty);
        }
    }

    public void Apply(Matrix4 modelMatrix)
    {
        Shader.Use(modelMatrix);

        foreach (var materialProperty in _materialProperties)
        {
            var location = materialProperty.ShaderUniformLocation;
            var value = materialProperty.Value;

            if (materialProperty.PropertyType == typeof(Vector4)) GL.Uniform4(location, (Vector4)value);
            if (materialProperty.PropertyType == typeof(Texture)) ((Texture)value).Use(0);
            // todo Add more types
        }
    }
}