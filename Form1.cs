using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKProject;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WinFormsOpenTK
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private System.Windows.Forms.Timer _fpsTimer;
        private System.Windows.Forms.Timer _renderTimer;
        private System.Windows.Forms.Timer _inputTimer;

        private Light _light;
        private CameraController _cameraController;
        private Renderer _renderer;
        private SceneInitializer _sceneInitializer;
        private List<SceneObject> _sceneObjects;
        private float _lastX, _lastY;
        private bool _firstMove = true;
        private bool _isMouseCaptured = true;
        private int _frameCount;
        private float _fps;
        private DateTime _lastFPSTime = DateTime.Now;
        private DataLoader dataLoader;

        private string _selectedModelPath = "";
        private string _selectedTexturePath = "";

        private const int VK_W = 0x57;
        private const int VK_A = 0x41;
        private const int VK_S = 0x53;
        private const int VK_D = 0x44;
        private const int VK_SPACE = 0x20;
        private const int VK_CONTROL = 0x11;
        private const int VK_ESCAPE = 0x1B;

        public Form1()
        {
            InitializeComponent();
            InitializeOpenGL();
            InitializeScene();
            SetupTimers();
            LoadData();

            //LoadModel("Models/Frog.obj", "Models/Textures/FrogTexture.jpg");
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

                // Clean up resources
                var obj = _sceneObjects[index];
                obj.Model.Shader?.Dispose();

                _sceneObjects.RemoveAt(index);
                listBox1.Items.RemoveAt(index);

                listBox1.Enabled = _sceneObjects.Count > 0;
                UpdateObjectsCount();

                Console.WriteLine($"Object removed. Remaining objects: {_sceneObjects.Count}");
            }
        }

        private void BtnAddObject_Click(object sender, EventArgs e)
        {
            try
            {
                var model = _sceneInitializer.CreateModel(_selectedModelPath, _selectedTexturePath);

                Random rand = new Random();
                Vector3 position = new Vector3(
                    rand.Next(-4, 5),
                    0f,
                    rand.Next(-4, 5)
                );

                SceneObject newObj;
                string objectTypeName;

                if (ModelType.SelectedIndex == 1)
                {
                    newObj = new PathMover(model, position, Vector3.Zero, new Vector3(1f, 1f, 1f));

                    Vector3[] pathPoints = new Vector3[]
                    {
                        new Vector3(position.X, position.Y, position.Z),
                        new Vector3(position.X + 3, position.Y, position.Z),
                        new Vector3(position.X + 3, position.Y, position.Z + 3),
                        new Vector3(position.X, position.Y, position.Z + 3)
                    };

                    ((PathMover)newObj).SetPoints(pathPoints);
                    objectTypeName = "Moving";
                }
                else
                {
                    Vector3 pos = new Vector3(float.Parse(xCoord.Text),
                        float.Parse(yCoord.Text),
                        float.Parse(zCoord.Text));
                    newObj = new SceneObject(model, pos, Vector3.Zero, new Vector3(1f, 1f, 1f));
                    objectTypeName = "Static";
                }

                newObj.Start();
                _sceneObjects.Add(newObj);

                string fileName = Path.GetFileNameWithoutExtension(_selectedModelPath);
                listBox1.Items.Add($"[{objectTypeName}] {fileName} ({position.X:F1}, {position.Y:F1}, {position.Z:F1})");

                DeleteModelsBtn.Enabled = true;
                UpdateObjectsCount();

                //_selectedModelPath = "";
                //_selectedTexturePath = "";
                //LoadButton.Enabled = false;

                Console.WriteLine($"Object added: {objectTypeName} at position {position}");
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
                    }
                }
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
            _light = new Light(new Vector3(-5f, 3.0f, 3.0f), Color.AntiqueWhite);
            _cameraController = new CameraController(5.0f, new Vector3(0.0f, 2.0f, 5.0f));

            // Убедитесь, что пути к шейдерам правильные
            string vertShaderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Shaders", "Vert", "shader.vert");
            string fragShaderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Shaders", "Frag", "shader.frag");

            if (!File.Exists(vertShaderPath) || !File.Exists(fragShaderPath))
            {
                vertShaderPath = "D:\\Development\\OpenTKProject\\Shaders\\Vert\\shader.vert";//"D:\\Projects\\VSProjects\\OpenTKProject\\Shaders\\Vert\\shader.vert";
                fragShaderPath = "D:\\Development\\OpenTKProject\\Shaders\\Frag\\shader.frag";//"D:\\Projects\\VSProjects\\OpenTKProject\\Shaders\\Frag\\shader.frag";
            }

            var modelFactory = new ModelFactory(vertShaderPath, fragShaderPath);
            _sceneInitializer = new SceneInitializer(modelFactory);
            _renderer = new Renderer(_cameraController, _light);

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

            if ((GetAsyncKeyState(VK_W) & 0x8000) != 0)
                _cameraController.MoveForward(deltaTime);
            if ((GetAsyncKeyState(VK_S) & 0x8000) != 0)
                _cameraController.MoveBackward(deltaTime);
            if ((GetAsyncKeyState(VK_A) & 0x8000) != 0)
                _cameraController.MoveLeft(deltaTime);
            if ((GetAsyncKeyState(VK_D) & 0x8000) != 0)
                _cameraController.MoveRight(deltaTime);
            if ((GetAsyncKeyState(VK_SPACE) & 0x8000) != 0)
                _cameraController.MoveUp(deltaTime);
            if ((GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0)
                _cameraController.MoveDown(deltaTime);
            if ((GetAsyncKeyState(VK_ESCAPE) & 0x8001) != 0)
            {
                _isMouseCaptured = false;
                Cursor.Show();
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
    }
}
