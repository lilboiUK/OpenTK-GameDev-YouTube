using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace OpenTK_Game;

public class Texture(int glHandle)
{
    public static Texture LoadFromFile(string path)
    {
        var handle = GL.GenTexture();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, handle);
        StbImage.stbi_set_flip_vertically_on_load(1);

        using (Stream stream = File.OpenRead(path))
        {
            var imageResult = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, imageResult.Width, imageResult.Height,
                0, PixelFormat.Rgba, PixelType.UnsignedByte, imageResult.Data);
        }

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        return new Texture(handle);
    }

    public void Use(TextureUnit textureUnit)
    {
        GL.ActiveTexture(textureUnit);
        GL.BindTexture(TextureTarget.Texture2D, glHandle);
    }

    public static Texture DefaultTexture()
    {
        return LoadFromFile("textures/default-texture.png");
    }
}