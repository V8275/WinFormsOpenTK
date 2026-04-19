using OpenTK.Graphics.OpenGL;

namespace OpenTKProject
{
    public class Model
    {
        VisualModel vModel;
        Texture texture;
        Texture normalMap;
        Texture metallic;
        Texture roughness;
        Shader shader;

        public VisualModel VModel { get { return vModel; } }
        public Texture Texture { get { return texture; } }
        public Texture NormalMap { get { return normalMap; } }
        public Texture Metallic { get { return metallic; } }
        public Texture Roughness { get { return roughness; } }
        public Shader Shader { get { return shader; } }

        public Model() { }
        public Model(VisualModel mod, Texture tex, Shader shad)
        {
            vModel = mod;
            texture = tex;
            shader = shad;
        }

        public Model(Model m, ModelFormat modelFormat = ModelFormat.Obj)
        {
            SetVModel(m.vModel.PathToModel, modelFormat);
            if(m.Texture != null)
                SetTexture(m.Texture.PathToTexture);
            SetShader(m.shader.PathToVecShader, m.shader.PathToFragShader);
        }

        public void SetVModel(string path, ModelFormat modelFormat)
        {
            vModel = new VisualModel(path, modelFormat);
        }

        public void SetTexture(string path)
        {
            texture = new Texture(path, true, PixelInternalFormat.SrgbAlpha);
        }

        public void SetNormalMap(string path)
        {
            normalMap = new Texture(path);
        }

        public void SetMetallic(string path)
        {
            metallic = new Texture(path, true);
        }

        public void SetRoughness(string path)
        {
            roughness = new Texture(path, true);
        }

        public void SetShader(string vertPath, string fragPath)
        {
            shader = new Shader(vertPath, fragPath);
        }
    }
}
