using OpenTK.Mathematics;

namespace OpenTKProject
{
    public class SceneInitializer
    {
        private readonly ModelFactory _modelFactory;

        public SceneInitializer(ModelFactory modelFactory)
        {
            _modelFactory = modelFactory;
        }

        public Model CreateModel(string modelPath, string texturePath)
        {
            return _modelFactory.CreateModel(modelPath, texturePath);
        }

        public List<SceneObject> CreateScene(Light light)
        {
            // Возвращаем пустую сцену
            return new List<SceneObject>();
        }
    }
}