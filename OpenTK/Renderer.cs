using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenTKProject
{
    public class Renderer
    {
        private readonly CameraController _cameraController;
        private readonly ShadowMapRenderer _shadowMapRenderer;
        private readonly ModelRenderer _modelRenderer;
        private readonly Light _light;

        public Renderer(CameraController cameraController, Light light, int shadowMapSize = 2048)
        {
            _cameraController = cameraController;
            _light = light;
            _shadowMapRenderer = new ShadowMapRenderer(shadowMapSize, shadowMapSize);
            _modelRenderer = new ModelRenderer();
        }

        public void Initialize()
        {
            // Инициализируем все OpenGL ресурсы после создания контекста
            _shadowMapRenderer.Initialize();
        }

        public void Render(List<SceneObject> sceneObjects, Vector2i windowSize)
        {
            // Render shadow map first
            _shadowMapRenderer.RenderShadowMap(sceneObjects, _light);

            // Render main scene
            GL.Viewport(0, 0, windowSize.X, windowSize.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var obj in sceneObjects)
            {
                _modelRenderer.RenderModel(obj, _cameraController, _light,
                    _shadowMapRenderer.ShadowMapTexture, windowSize);
            }
        }
    }
}