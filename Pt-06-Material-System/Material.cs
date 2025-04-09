using OpenTK.Graphics.ES20;
using OpenTK.Mathematics;

namespace OpenTK_Game;

public class Material
{
    private readonly List<MaterialProperty> _properties = [];
    private Shader _shader;

    public Material(Shader shader)
    {
        _shader = shader;
        UpdatePropertiesList();
    }

    private void UpdatePropertiesList()
    {
        _properties.Clear();
        for (var i = 0; i < _shader.GetUniformsCount(); i++)
        {
            var name = _shader.GetUniformName(i);
            if(name == "transform") continue; // todo Find better solution
            var location = _shader.GetUniformLocation(name);
            var activeUniformType = _shader.GetActiveUniformType(i);
            var materialProperty = new MaterialProperty(name, location, activeUniformType);
            _properties.Add(materialProperty);
        }
    }

    public void SetShader(Shader shader)
    {
        _shader = shader;
        UpdatePropertiesList();
    }

    public void SetProperty(string propertyName, object value)
    {
        foreach(var property in _properties.Where(property => property.Name == propertyName))
            property.Value = value;
    }

    public void Apply(Matrix4 modelMatrix)
    {
        _shader.Use(modelMatrix);

        foreach (var property in _properties)
        {
            var location = property.ShaderUniformLocation;
            var value = property.Value;
            
            if(property.PropertyType == typeof(Vector4))
                GL.Uniform4(location, (Vector4)value);
            // todo Add more types
        }
    }
}