using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKProject;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace WinFormsOpenTK
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private System.Windows.Forms.Timer _fpsTimer;
        private System.Windows.Forms.Timer _renderTimer;
        private System.Windows.Forms.Timer _inputTimer;

        string vertShaderPath = "WinFormsOpenTK.Shaders.Vert.shader.vert";
        string fragShaderPath = "WinFormsOpenTK.Shaders.Frag.shader.frag";

        private CameraController _cameraController;
        private Renderer _renderer;
        private SceneInitializer _sceneInitializer;
        private List<SceneObject> _sceneObjects;
        private PhysicsWorld _physicsWorld;
        private float _lastX, _lastY;
        private bool _firstMove = true;
        private bool _isMouseCaptured = true;
        private int _frameCount;
        private float _fps;
        private DateTime _lastFPSTime = DateTime.Now;
        private DataLoader dataLoader;

        private string _selectedModelPath = "";
        private string _selectedTexturePath = "";
        private string _selectedModelFormat;
        private bool isPhysicsEnabled;
        private bool isPhysicsKinematic;
        private bool isCollide;
        private bool isMoveEnable;

        private List<SceneObject> lightObject = new List<SceneObject>();

        public Form1()
        {
            InitializeComponent();
            InitializeOpenGL();
            InitializeScene();
            SetupTimers();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dataLoader = new DataLoader();
                dataLoader.LoadData();

                var data = dataLoader.GetLoadedData();
                List<string> names = new List<string>();

                for (int i = 0; i < data.Count; i++)
                {
                    names.Add(data[i].PresetName.ToString());
                }

                comboBox1.DataSource = names;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadModel(string modelPath, string texturePath)
        {
            _selectedModelPath = modelPath;
            _selectedTexturePath = texturePath;

            UpdateAddButtonState();
        }

        private void UpdateAddButtonState()
        {
            LoadButton.Enabled = !string.IsNullOrEmpty(_selectedModelPath) &&
                                    !string.IsNullOrEmpty(_selectedTexturePath) &&
                                    File.Exists(_selectedModelPath) &&
                                    File.Exists(_selectedTexturePath);
        }

        private void DeleteModels_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0 &&
                listBox1.SelectedIndex < _sceneObjects.Count)
            {
                int index = listBox1.SelectedIndex;

                var obj = _sceneObjects[index];
                obj.Model.Shader?.Dispose();

                _sceneObjects.RemoveAt(index);
                listBox1.Items.RemoveAt(index);

                listBox1.Enabled = _sceneObjects.Count > 0;
                UpdateObjectsCount();

                Console.WriteLine($"Object removed. Remaining objects: {_sceneObjects.Count}");
            }
        }

        private List<ObjectModule> CreateModules(SceneObject currentObject)
        {
            List<ObjectModule> modules = new List<ObjectModule>();
            if (isPhysicsEnabled)
            {
                float m = 10;

                if (!string.IsNullOrWhiteSpace(massText.Text))
                {
                    if (float.TryParse(massText.Text, out float result))
                    {
                        m = result;
                    }
                }

                modules.Add(ModuleInitializer.AddPhysicsModule(currentObject, _physicsWorld, isPhysicsKinematic, m));

                if (isMoveEnable)
                {
                    float s = 10;

                    if (!string.IsNullOrWhiteSpace(speedBox.Text))
                    {
                        if (float.TryParse(speedBox.Text, out float result))
                        {
                            s = result;
                        }
                    }

                    modules.Add(ModuleInitializer.AddMoveModule(currentObject, s));
                }
            }
            

            if (isCollide)
            {
                Vector3 colliderSize = CalculateModelBounds(currentObject.Model);

                modules.Add(ModuleInitializer.AddCollisionModule(currentObject, _physicsWorld, colliderSize));
            }

            return modules;
        }

        private Vector3 CalculateModelBounds(Model model)
        {
            if (model?.VModel?.Vertices == null)
                return new Vector3(1f, 1f, 1f);

            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;

            var vertices = model.VModel.Vertices;
            for (int i = 0; i < vertices.Count; i += 8)
            {
                if (i + 2 >= vertices.Count) break;

                float x = vertices[i];
                float y = vertices[i + 1];
                float z = vertices[i + 2];

                minX = Math.Min(minX, x);
                maxX = Math.Max(maxX, x);
                minY = Math.Min(minY, y);
                maxY = Math.Max(maxY, y);
                minZ = Math.Min(minZ, z);
                maxZ = Math.Max(maxZ, z);
            }

            return new Vector3(
                maxX - minX,
                maxY - minY,
                maxZ - minZ
            );
        }

        private void BtnAddObject_Click(object sender, EventArgs e)
        {
            try
            {
                var model = _sceneInitializer.CreateModel(_selectedModelPath, _selectedTexturePath, GetFormatFromString(_selectedModelFormat));

                Vector3 position = new Vector3(float.Parse(xCoord.Text),
                        float.Parse(yCoord.Text),
                        float.Parse(zCoord.Text));

                Vector3 scale = new Vector3(float.Parse(xScale.Text),
                        float.Parse(yScale.Text),
                        float.Parse(zScale.Text));

                SceneObject newObj;

                newObj = new SceneObject(model, position, Vector3.Zero, scale);

                newObj.MakeModuleList(CreateModules(newObj));

                newObj.Start();
                _sceneObjects.Add(newObj);

                string fileName = Path.GetFileNameWithoutExtension(_selectedModelPath);
                listBox1.Items.Add($"{fileName} ({position.X:F1}, {position.Y:F1}, {position.Z:F1})");

                DeleteModelsBtn.Enabled = true;
                UpdateObjectsCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding object: {ex.Message}\n\nStack trace: {ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = dataLoader.GetLoadedData();

            if (data != null)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].PresetName == comboBox1.SelectedItem.ToString())
                    {
                        _selectedModelPath = data[i].ModelPath;
                        _selectedTexturePath = data[i].TexturePath;
                        _selectedModelFormat = data[i].ModelFormat;
                    }
                }
            }
        }

        private ModelFormat GetFormatFromString(string formatType)
        {
            switch (formatType)
            {
                case "obj":
                    return ModelFormat.Obj;
                case "gltf":
                    return ModelFormat.Gltf;
                default:
                    return ModelFormat.Obj;
            }
        }

        private void InitializeOpenGL()
        {
            _glControl.Load += GlControl_Load;
            _glControl.Paint += GlControl_Paint;
            _glControl.Resize += GlControl_Resize;
            _glControl.MouseDown += GlControl_MouseDown;
            _glControl.MouseMove += GlControl_MouseMove;
            _glControl.MouseUp += GlControl_MouseUp;
            _glControl.LostFocus += GlControl_LostFocus;
            _glControl.MouseWheel += GlControl_MouseWheel;
            _glControl.TabStop = true;
        }

        private void GlControl_Load(object sender, EventArgs e)
        {
            _glControl.MakeCurrent();

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _renderer.Initialize();

            _inputTimer.Start();
            _renderTimer.Start();
            _fpsTimer.Start();

            Console.WriteLine("OpenGL initialized");

            _physicsWorld.Initialize();

            Console.WriteLine("Physics initialized");
        }

        private void GlControl_Paint(object sender, PaintEventArgs e) { }

        private void GlControl_Resize(object sender, EventArgs e)
        {
            if (_glControl.IsDisposed) return;
            _glControl.MakeCurrent();
            GL.Viewport(0, 0, _glControl.Width, _glControl.Height);
        }

        private void GlControl_MouseDown(object sender, MouseEventArgs e)
        {
            _glControl.Focus();
            _isMouseCaptured = true;
            _firstMove = true;
            Cursor.Hide();
        }

        private void GlControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMouseCaptured) return;

            if (_firstMove)
            {
                _lastX = e.X;
                _lastY = e.Y;
                _firstMove = false;
                return;
            }

            float xOffset = e.X - _lastX;
            float yOffset = _lastY - e.Y;

            _lastX = e.X;
            _lastY = e.Y;

            _cameraController.RotateCamera(xOffset, yOffset);

            if (_isMouseCaptured)
            {
                var centerX = _glControl.Width / 2;
                var centerY = _glControl.Height / 2;

                if (Math.Abs(e.X - centerX) > 100 || Math.Abs(e.Y - centerY) > 100)
                {
                    Cursor.Position = _glControl.PointToScreen(new Point(centerX, centerY));
                    _lastX = centerX;
                    _lastY = centerY;
                }
            }
        }

        private void GlControl_MouseUp(object sender, MouseEventArgs e) { }

        private void GlControl_LostFocus(object sender, EventArgs e)
        {
            _isMouseCaptured = false;
            Cursor.Show();
        }

        private void InitializeScene()
        {
            var light = new SceneObject(null, new Vector3(-5f, 3.0f, 3.0f));
            var lightModule = new LightModule(light, Color.AntiqueWhite);
            light.AddModule(lightModule);
            lightObject.Add(light);

            _cameraController = new CameraController(5.0f, new Vector3(0.0f, 2.0f, 5.0f));
            _physicsWorld = new PhysicsWorld();

            var modelFactory = new ModelFactory(vertShaderPath, fragShaderPath);
            _sceneInitializer = new SceneInitializer(modelFactory);
            _renderer = new Renderer(_cameraController, lightObject[0].GetModule<LightModule>());

            _sceneObjects = new List<SceneObject>();
        }

        private void SetupTimers()
        {
            _fpsTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _fpsTimer.Tick += (s, e) =>
            {
                fPSCounter.Text = $"FPS: {_fps:F1}";
                _frameCount = 0;

                UpdateObjectsCount();
            };

            _inputTimer = new System.Windows.Forms.Timer { Interval = 5 };
            _inputTimer.Tick += (s, e) =>
            {
                if (_isMouseCaptured && !_glControl.IsDisposed)
                {
                    ProcessInput();

                    float deltaTime = _inputTimer.Interval / 1000.0f;
                    _physicsWorld.Update(deltaTime);
                }
            };

            _renderTimer = new System.Windows.Forms.Timer { Interval = 8 };
            _renderTimer.Tick += (s, e) =>
            {
                if (_glControl.IsDisposed) return;

                try
                {
                    _glControl.MakeCurrent();

                    foreach (var obj in _sceneObjects)
                    {
                        obj.Update(0.008f);
                    }

                    _renderer.Render(_sceneObjects, new Vector2i(_glControl.Width, _glControl.Height));
                    _glControl.SwapBuffers();

                    _frameCount++;
                    _fps = (float)(_frameCount / (DateTime.Now - _lastFPSTime).TotalSeconds);
                    _lastFPSTime = DateTime.Now;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Render error: {ex.Message}");
                }
            };
        }

        private void UpdateObjectsCount() { }

        private void ProcessInput()
        {
            float deltaTime = _inputTimer.Interval / 1000.0f;

            if (_cameraController.CurrentMode == CameraMode.Free)
            {
                if ((GetAsyncKeyState(KeyStates.VK_W) & 0x8000) != 0) _cameraController.MoveForward(deltaTime);
                if ((GetAsyncKeyState(KeyStates.VK_S) & 0x8000) != 0) _cameraController.MoveBackward(deltaTime);
                if ((GetAsyncKeyState(KeyStates.VK_A) & 0x8000) != 0) _cameraController.MoveLeft(deltaTime);
                if ((GetAsyncKeyState(KeyStates.VK_D) & 0x8000) != 0) _cameraController.MoveRight(deltaTime);
            }

            // Эти клавиши работают в обоих режимах
            if ((GetAsyncKeyState(KeyStates.VK_SPACE) & 0x8000) != 0) _cameraController.MoveUp(deltaTime);
            if ((GetAsyncKeyState(KeyStates.VK_CONTROL) & 0x8000) != 0) _cameraController.MoveDown(deltaTime);

            if ((GetAsyncKeyState(KeyStates.VK_ESCAPE) & 0x8001) != 0)
            {
                _isMouseCaptured = false;
                Cursor.Show();
            }
        }

        private void GlControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_isMouseCaptured)
            {
                // e.Delta равно 120 за один шаг колесика
                float delta = e.Delta / 120.0f;
                _cameraController.Zoom(delta);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _inputTimer?.Stop();
            _renderTimer?.Stop();
            _fpsTimer?.Stop();

            foreach (var obj in _sceneObjects)
            {
                obj.Model.Shader?.Dispose();
            }

            _physicsWorld?.Dispose();
            _glControl?.Dispose();
        }

        private void Coord_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            string newText = textBox.Text.Substring(0, textBox.SelectionStart) +
                e.KeyChar +
                textBox.Text.Substring(textBox.SelectionStart + textBox.SelectionLength);

            Regex reg = new Regex(@"^-?\d*\.?\d*$");

            if (!reg.IsMatch(newText))
                e.Handled = true;
        }

        private void isPhysicsAdded(object sender, EventArgs e)
        {
            var check = sender as CheckBox;
            isPhysicsEnabled = check.Checked;
        }

        private void isObjectKinematic(object sender, EventArgs e)
        {
            var check = sender as CheckBox;
            isPhysicsKinematic = check.Checked;
        }

        private void isCollision(object sender, EventArgs e)
        {
            var check = sender as CheckBox;
            isCollide = check.Checked;
        }

        private void isMove(object sender, EventArgs e)
        {
            var check = sender as CheckBox;
            isMoveEnable = check.Checked;
        }

        private void BindCamToSelect(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0 &&
                listBox1.SelectedIndex < _sceneObjects.Count)
            {
                int index = listBox1.SelectedIndex;

                var obj = _sceneObjects[index];

                _cameraController.SetOrbitMode(obj, 8.0f);
            }
        }

        private void UnbindCam(object sender, EventArgs e)
        {
            _cameraController.SetFreeMode();
            listBox1.ClearSelected();
        }
    }

    public static class ModuleInitializer
    {
        public static PhysicsModule AddPhysicsModule(SceneObject sceneObject, PhysicsWorld world,  bool isKinematic, float mass = 10)
        {
            return new PhysicsModule(sceneObject, world, mass, isKinematic);
        }

        public static CollisionModule AddCollisionModule(SceneObject sceneObject, PhysicsWorld world, Vector3 vector3)
        {
            return new CollisionModule(sceneObject, world, vector3);
        }

        public static MoveModule AddMoveModule(SceneObject sceneObject, float speed = 10)
        {
            return new MoveModule(sceneObject, speed);
        }
    }

    public static class KeyStates
    {
        public static int VK_W = 0x57;
        public static int VK_A = 0x41;
        public static int VK_S = 0x53;
        public static int VK_D = 0x44;
        public static int VK_SPACE = 0x20;
        public static int VK_CONTROL = 0x11;
        public static int VK_ESCAPE = 0x1B;
        public static int VK_Q = 0x51;
        public static int VK_E = 0x45;
        public static int VK_SHIFT = 0x10;
    }
}
