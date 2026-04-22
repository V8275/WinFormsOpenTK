using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace OpenTKProject
{
    public class SkyboxTexture
    {
        public int Handle { get; private set; }
        private string[] _faces;

        public SkyboxTexture(string[] facesPaths)
        {
            _faces = facesPaths;
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, Handle);

            StbImage.stbi_set_flip_vertically_on_load(0); // Для cubemap не переворачиваем

            for (int i = 0; i < facesPaths.Length; i++)
            {
                using var stream = File.OpenRead(facesPaths[i]);
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, 
                    PixelInternalFormat.SrgbAlpha, image.Width, image.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            
            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
        }
    }
}