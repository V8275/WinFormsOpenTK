using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

namespace OpenTKProject
{
    public class MoveModule : ObjectModule
    {
        private SceneObject _owner;
        private PhysicsModule _physicsModule;
        private KeyboardState _keyboard;
        private float _moveSpeed = 5.0f;
        private bool _usePhysics = true;

        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        public bool UsePhysics
        {
            get => _usePhysics;
            set => _usePhysics = value;
        }

        public MoveModule(SceneObject owner, KeyboardState keyboard, float moveSpeed = 5.0f)
        {
            _owner = owner;
            _keyboard = keyboard;
            _moveSpeed = moveSpeed;
        }

        public override void Start()
        {
            _physicsModule = _owner.GetModule<PhysicsModule>();
        }

        public override void Update(float time)
        {
            if (_keyboard == null) return;

            Vector3 movement = Vector3.Zero;

            if (_keyboard.IsKeyDown(Keys.W))
                movement.Z -= _moveSpeed * time;
            if (_keyboard.IsKeyDown(Keys.S))
                movement.Z += _moveSpeed * time;

            if (_keyboard.IsKeyDown(Keys.A))
                movement.X -= _moveSpeed * time;
            if (_keyboard.IsKeyDown(Keys.D))
                movement.X += _moveSpeed * time;

            if (_keyboard.IsKeyDown(Keys.Q))
                movement.Y -= _moveSpeed * time;
            if (_keyboard.IsKeyDown(Keys.E))
                movement.Y += _moveSpeed * time;

            if (_usePhysics && _physicsModule != null && _physicsModule.RigidBody != null)
            {
                MoveWithPhysics(movement);
            }
            else
            {
                MoveDirect(movement);
            }
        }

        private void MoveWithPhysics(Vector3 movement)
        {
            if (movement == Vector3.Zero) return;

            var rb = _physicsModule.RigidBody;

            if (rb.IsKinematicObject)
            {
                var newPos = _owner.Position + movement;
                rb.WorldTransform = BulletSharp.Math.Matrix.Translation(
                    newPos.X, newPos.Y, newPos.Z);
                _owner.Position = newPos;
            }
            else
            {
                Vector3 force = movement * 10.0f;
                _physicsModule.ApplyForce(force);
            }
        }

        private void MoveDirect(Vector3 movement)
        {
            _owner.Position += movement;
        }

        /// <summary>
        /// Обновить ссылку на клавиатуру (если нужно)
        /// </summary>
        public void UpdateKeyboard(KeyboardState keyboard)
        {
            _keyboard = keyboard;
        }

        /// <summary>
        /// Движение в определенном направлении (для использования из другого кода)
        /// </summary>
        public void MoveInDirection(Vector3 direction, float deltaTime)
        {
            Vector3 movement = direction.Normalized() * _moveSpeed * deltaTime;

            if (_usePhysics && _physicsModule != null)
            {
                MoveWithPhysics(movement);
            }
            else
            {
                MoveDirect(movement);
            }
        }
    }
}