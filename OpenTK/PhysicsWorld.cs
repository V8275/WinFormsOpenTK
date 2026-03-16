using BulletSharp;
using Matrix = BulletSharp.Math.Matrix;
using BulletVector3 = BulletSharp.Math.Vector3;
using OpenTKVector3 = OpenTK.Mathematics.Vector3;

namespace OpenTKProject
{
    public class PhysicsWorld : IDisposable
    {
        private DiscreteDynamicsWorld _world;
        private CollisionDispatcher _dispatcher;
        private DbvtBroadphase _broadphase;
        private SequentialImpulseConstraintSolver _solver;
        private CollisionConfiguration _collisionConfig;

        private Dictionary<SceneObject, RigidBody> _rigidBodies;
        private bool _disposed;
        private bool _isInitialized;

        public PhysicsWorld()
        {
            _rigidBodies = new Dictionary<SceneObject, RigidBody>();
        }

        public void Initialize()
        {
            if (_isInitialized) return;

            _collisionConfig = new DefaultCollisionConfiguration();
            _dispatcher = new CollisionDispatcher(_collisionConfig);
            _broadphase = new DbvtBroadphase();
            _solver = new SequentialImpulseConstraintSolver();

            _world = new DiscreteDynamicsWorld(_dispatcher, _broadphase, _solver, _collisionConfig);
            _world.Gravity = new BulletVector3(0, -9.81f, 0);

            _isInitialized = true;
        }

        public void Update(float deltaTime)
        {
            if (!_isInitialized) return;

            _world.StepSimulation(deltaTime, 10, 1f / 60f);

            // Обновляем позиции SceneObject из физического мира
            foreach (var pair in _rigidBodies)
            {
                if (pair.Value != null && pair.Value.MotionState != null)
                {
                    var motionState = (DefaultMotionState)pair.Value.MotionState;
                    var transform = motionState.WorldTransform;

                    // Конвертируем BulletSharp матрицу в OpenTK позицию
                    var openTKPos = new OpenTKVector3(
                        transform.Origin.X,
                        transform.Origin.Y,
                        transform.Origin.Z
                    );

                    // Обновляем позицию SceneObject
                    pair.Key.Position = openTKPos;
                }
            }
        }

        public RigidBody AddRigidBody(SceneObject obj, float mass = 1.0f, bool isKinematic = false)
        {
            if (!_isInitialized) Initialize();

            if (_rigidBodies.ContainsKey(obj))
                return _rigidBodies[obj];

            // Создаем коллизионную форму на основе модели
            var collisionShape = CreateCollisionShape(obj);

            // Вычисляем локальную инерцию
            BulletVector3 localInertia = BulletVector3.Zero;
            if (mass > 0)
                collisionShape.CalculateLocalInertia(mass, out localInertia);

            // Создаем трансформацию начальной позиции
            var startTransform = Matrix.Translation(
                obj.Position.X,
                obj.Position.Y,
                obj.Position.Z
            );

            // Создаем motion state
            var motionState = new DefaultMotionState(startTransform);

            // Создаем rigid body
            var rbInfo = new RigidBodyConstructionInfo(mass, motionState, collisionShape, localInertia);
            var body = new RigidBody(rbInfo);

            if (isKinematic)
            {
                body.CollisionFlags |= CollisionFlags.KinematicObject;
                body.ActivationState = ActivationState.DisableDeactivation;
            }

            _world.AddRigidBody(body);
            _rigidBodies[obj] = body;

            return body;
        }

        private CollisionShape CreateCollisionShape(SceneObject obj)
        {
            var halfExtents = CalculateModelBounds(obj.Model) * 0.5f;

            return new BoxShape(new BulletVector3(
                halfExtents.X * obj.Scale.X,
                halfExtents.Y * obj.Scale.Y,
                halfExtents.Z * obj.Scale.Z
            ));
        }

        private OpenTKVector3 CalculateModelBounds(Model model)
        {
            if (model?.VModel?.Vertices == null)
                return new OpenTKVector3(1, 1, 1);

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

            return new OpenTKVector3(
                maxX - minX,
                maxY - minY,
                maxZ - minZ
            );
        }

        public DiscreteDynamicsWorld GetWorld()
        {
            if (!_isInitialized) Initialize();
            return _world;
        }

        public void RemoveRigidBody(SceneObject obj)
        {
            if (_rigidBodies.TryGetValue(obj, out var body))
            {
                _world.RemoveRigidBody(body);
                body.Dispose();
                _rigidBodies.Remove(obj);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            foreach (var body in _rigidBodies.Values)
                body.Dispose();

            _world?.Dispose();
            _solver?.Dispose();
            _broadphase?.Dispose();
            _dispatcher?.Dispose();
            _collisionConfig?.Dispose();

            _disposed = true;
        }
    }
}