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

        public Model CreateModel(string modelPath, string texturePath = "", ModelFormat modelFormat = ModelFormat.Obj, 
                                 string vertShader = "", string fragShader = "")
        {
            Model model = new Model();
            model.SetVModel(modelPath, modelFormat);
            
            if (!string.IsNullOrEmpty(texturePath))
                model.SetTexture(texturePath);

            if (string.IsNullOrEmpty(vertShader) || string.IsNullOrEmpty(fragShader))
                model.SetShader(_defaultVertShader, _defaultFragShader);
            else
                model.SetShader(vertShader, fragShader);

            return model;
        }
    }
}