using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace OpenTKProject
{
    public class OpenWindow : GameWindow
    {
        private readonly Light _light;
        private readonly CameraController _cameraController;
        private readonly Renderer _renderer;
        private readonly SceneInitializer _sceneInitializer;
        private PhysicsWorld _physicsWorld;

        private List<SceneObject> _sceneObjects;
        private float _lastX, _lastY;
        private bool _firstMove = true;

        public OpenWindow(int width, int height, string title) :
            base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                ClientSize = (width, height),
                Title = title
            })
        {
            _light = new Light(new Vector3(-5f, 3.0f, 3.0f), Color.AntiqueWhite);
            _cameraController = new CameraController(1.5f, new Vector3(0.0f, 0.0f, 3.0f));
            _physicsWorld = new PhysicsWorld(); 

            var modelFactory = new ModelFactory(
                "D:\\Projects\\VSProjects\\OpenTKProject\\Shaders\\Vert\\shader.vert",
                "D:\\Projects\\VSProjects\\OpenTKProject\\Shaders\\Frag\\shader.frag");

            _sceneInitializer = new SceneInitializer(modelFactory);
            _renderer = new Renderer(_cameraController, _light);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            CursorState = CursorState.Grabbed;

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _sceneObjects = _sceneInitializer.CreateScene(_light);

            _physicsWorld.Initialize();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            KeyboardState input = KeyboardState;

            _physicsWorld.Update((float)e.Time);

            _renderer.Render(_sceneObjects, Size);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            _sceneObjects[1].Update((float)e.Time);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (_firstMove)
            {
                _lastX = e.X;
                _lastY = e.Y;
                _firstMove = false;
            }

            float xOffset = e.X - _lastX;
            float yOffset = _lastY - e.Y;

            _lastX = e.X;
            _lastY = e.Y;

            _cameraController.RotateCamera(xOffset, yOffset);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            _physicsWorld?.Dispose();

            foreach (var obj in _sceneObjects)
            {
                obj.Model.Shader.Dispose();
            }

            base.OnUnload();
        }

        public new Vector2i Size
        {
            get => base.Size;
            set => base.Size = value;
        }
    }
}