using OpenTK.Mathematics;

namespace OpenTKProject
{
    public class SceneObject
    {
        public Model Model { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public List<ObjectModule> Modules { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Model's geometry</param>
        /// <param name="position">Model' start position</param>
        /// <param name="newmodules">Model's start array of extension code modules</param>
        public SceneObject(Model model, Vector3 position, List<ObjectModule> newmodules = null)
        {
            Model = model;
            Position = position;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
            MakeModuleList(newmodules);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Model's geometry</param>
        /// <param name="position">Model' start position</param>
        /// <param name="scale">Model's start scale</param>
        /// <param name="newmodules">Model's start array of extension code modules</param>
        public SceneObject(Model model, Vector3 position, Vector3 scale, List<ObjectModule> newmodules = null)
        {
            Model = model;
            Position = position;
            Rotation = Vector3.Zero;
            Scale = scale;
            MakeModuleList(newmodules);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Model's geometry</param>
        /// <param name="position">Model' start position</param>
        /// <param name="rotation">Model's start rotation</param>
        /// <param name="scale">Model's start scale</param>
        /// <param name="newmodules">Model's start array of extension code modules</param>
        public SceneObject(Model model, Vector3 position, Vector3 rotation, Vector3 scale, List<ObjectModule> newmodules = null)
        {
            Model = model;
            Position = position;
            Rotation = rotation;
            Scale = scale;
            MakeModuleList(newmodules);
        }

        public virtual void Start()
        {
            if(Modules != null)
                foreach(ObjectModule module in Modules)
                {
                    module.Start();
                }
        }

        public virtual void Update(float time)
        {
            if (Modules != null)
                foreach (ObjectModule module in Modules)
                {
                    module.Update(time);
                }
        }

        /// <summary>
        /// Add extension module to object
        /// </summary>
        /// <param name="module"></param>
        public void AddModule(ObjectModule module)
        {
            if (Modules != null) Modules.Add(module);
            else 
            {
                Modules = new List<ObjectModule>();
                Modules.Add(module);
            }
        }

        protected void MakeModuleList(List<ObjectModule> newmodules)
        {
            if (newmodules != null) Modules = newmodules;
            else Modules = new List<ObjectModule>();
        }

        public Matrix4 GetModelMatrix()
        {
            return Matrix4.CreateScale(Scale) *
                   Matrix4.CreateRotationX(Rotation.X) *
                   Matrix4.CreateRotationY(Rotation.Y) *
                   Matrix4.CreateRotationZ(Rotation.Z) *
                   Matrix4.CreateTranslation(Position);
        }
    }
}