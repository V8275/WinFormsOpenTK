using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenTKProject
{
    public class Renderer
    {
        private readonly CameraController _cameraController;
        private readonly ShadowMapRenderer _shadowMapRenderer;
        private readonly ModelRenderer _modelRenderer;
        private readonly LightModule _light;

        public Renderer(CameraController cameraController, LightModule light, int shadowMapSize = 2048)
        {
            _cameraController = cameraController;
            _light = light;
            _shadowMapRenderer = new ShadowMapRenderer(shadowMapSize, shadowMapSize);
            _modelRenderer = new ModelRenderer();
        }

        public void Initialize()
        {
            _shadowMapRenderer.Initialize();
        }

        public void Render(List<SceneObject> sceneObjects, Vector2i windowSize)
        {
            _shadowMapRenderer.RenderShadowMap(sceneObjects, _light);

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