using BulletSharp;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace OpenTKProject
{
    public class CollisionModule : ObjectModule
    {
        private SceneObject _owner;
        private PhysicsWorld _physicsWorld;
        private CollisionObject _collisionObject;
        private Vector3 _colliderSize;
        private CollisionShape _collisionShape;

        public CollisionObject CollisionObject => _collisionObject;

        public CollisionModule(SceneObject owner, PhysicsWorld physicsWorld, Vector3 colliderSize)
        {
            _owner = owner;
            _physicsWorld = physicsWorld;
            _colliderSize = colliderSize;
        }

        public override void Start()
        {
            if (_physicsWorld == null) return;

            var world = _physicsWorld.GetWorld();
            if (world == null) return;

            try
            {
                _collisionShape = new BoxShape(new BulletSharp.Math.Vector3(
                    _colliderSize.X * _owner.Scale.X / 2f,
                    _colliderSize.Y * _owner.Scale.Y / 2f,
                    _colliderSize.Z * _owner.Scale.Z / 2f
                ));

                _collisionObject = new CollisionObject();
                _collisionObject.CollisionShape = _collisionShape;
                _collisionObject.CollisionFlags = CollisionFlags.StaticObject;
                _collisionObject.UserObject = _owner;

                var transform = BulletSharp.Math.Matrix.Translation(
                    _owner.Position.X,
                    _owner.Position.Y,
                    _owner.Position.Z
                );
                _collisionObject.WorldTransform = transform;

                world.AddCollisionObject(_collisionObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating collision object: {ex.Message}");
            }
        }

        public override void Update(float time)
        {
            if (_collisionObject != null)
            {
                var transform = BulletSharp.Math.Matrix.Translation(
                    _owner.Position.X,
                    _owner.Position.Y,
                    _owner.Position.Z
                );
                _collisionObject.WorldTransform = transform;
            }
        }

        public void Dispose()
        {
            if (_collisionObject != null && _physicsWorld?.GetWorld() != null)
            {
                _physicsWorld.GetWorld().RemoveCollisionObject(_collisionObject);
                _collisionObject.Dispose();
            }

            if (_collisionShape != null)
            {
                _collisionShape.Dispose();
            }
        }
    }
}