namespace OpenTKProject
{
    public class ModelFactory
    {
        private readonly string _defaultVertShader;
        private readonly string _defaultFragShader;

        public ModelFactory(string defaultVertShader, string defaultFragShader)
        {
            _defaultVertShader = defaultVertShader;
            _defaultFragShader = defaultFragShader;
        }

        public Model CreateModel(string modelPath, ModelTextures modelTextures, ModelFormat modelFormat = ModelFormat.Obj, 
                                 string vertShader = "", string fragShader = "")
        {
            Model model = new Model();
            model.SetVModel(modelPath, modelFormat);
            
            if (!string.IsNullOrEmpty(modelTextures.TexturePath))
                model.SetTexture(modelTextures.TexturePath);

            if (!string.IsNullOrEmpty(modelTextures.NormalMapPath))
                model.SetNormalMap(modelTextures.NormalMapPath);

            if (!string.IsNullOrEmpty(modelTextures.MetallicMapPath))
                model.SetMetallic(modelTextures.MetallicMapPath);

            if (!string.IsNullOrEmpty(modelTextures.RoughnessMapPath))
                model.SetRoughness(modelTextures.RoughnessMapPath);

            if (string.IsNullOrEmpty(vertShader) || string.IsNullOrEmpty(fragShader))
                model.SetShader(_defaultVertShader, _defaultFragShader);
            else
                model.SetShader(vertShader, fragShader);

            return model;
        }
    }

    public struct ModelTextures
    {
        public string TexturePath;
        public string NormalMapPath;
        public string MetallicMapPath;
        public string RoughnessMapPath;
    }
}