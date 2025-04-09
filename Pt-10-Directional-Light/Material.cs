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
            // todo Find better solution
            var name = Shader.GetUniformName(i);
            switch (name)
            {
                case "model":
                case "view":
                case "projection":
                case "lightDir":
                case "lightColor":
                    continue;
            }

            var location = Shader.GetUniformLocation(name);
            var activeUniformType = Shader.GetActiveUniformType(i);
            var materialProperty = new MaterialProperty(name, location, activeUniformType);
            _materialProperties.Add(materialProperty);
        }
    }

    public void Apply(Matrix4 model, Matrix4 view, Matrix4 projection, Light light)
    {
        Shader.Use(model, view, projection, light);

        foreach (var materialProperty in _materialProperties)
        {
            var location = materialProperty.ShaderUniformLocation;
            var value = materialProperty.Value;

            if (materialProperty.PropertyType == typeof(Vector2)) GL.Uniform2(location, (Vector2)value);
            if (materialProperty.PropertyType == typeof(Vector3)) GL.Uniform3(location, (Vector3)value);
            if (materialProperty.PropertyType == typeof(Texture)) ((Texture)value).Use(0);
            // todo Add more types
        }
    }
}