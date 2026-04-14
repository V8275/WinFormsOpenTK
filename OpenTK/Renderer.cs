using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKProject;

public class Renderer
{
    private readonly CameraController _cameraController;
    private readonly ShadowMapRenderer _shadowMapRenderer;
    private readonly ModelRenderer _modelRenderer;
    private readonly LightManager _lightManager;

    public Renderer(CameraController cameraController, LightManager lightManager, int shadowMapSize = 2048)
    {
        _cameraController = cameraController;
        _lightManager = lightManager;
        _shadowMapRenderer = new ShadowMapRenderer(shadowMapSize, shadowMapSize);
        _modelRenderer = new ModelRenderer();
    }

    public void Initialize()
    {
        _shadowMapRenderer.Initialize();
    }

    public void Render(List<SceneObject> sceneObjects, Vector2i windowSize)
    {
        var mainLight = _lightManager.GetLights().FirstOrDefault();
        if (mainLight != null)
        {
            _shadowMapRenderer.RenderShadowMap(sceneObjects, mainLight);
        }

        GL.Viewport(0, 0, windowSize.X, windowSize.Y);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        foreach (var obj in sceneObjects)
        {
            _modelRenderer.RenderModel(obj, _cameraController, _lightManager,
                _shadowMapRenderer.ShadowMapTexture, windowSize);
        }
    }
}