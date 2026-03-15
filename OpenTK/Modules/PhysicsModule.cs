using OpenTK.Mathematics;
using BulletSharp;

namespace OpenTKProject
{
    public class PhysicsModule : ObjectModule
    {
        private SceneObject _owner;
        private PhysicsWorld _physicsWorld;
        private RigidBody _rigidBody;
        private float _mass;
        private bool _isKinematic;

        public RigidBody RigidBody => _rigidBody;

        public PhysicsModule(SceneObject owner, PhysicsWorld physicsWorld, float mass = 1.0f, bool isKinematic = false)
        {
            _owner = owner;
            _physicsWorld = physicsWorld;
            _mass = mass;
            _isKinematic = isKinematic;
        }

        public override void Start()
        {
            _rigidBody = _physicsWorld.AddRigidBody(_owner, _mass, _isKinematic);
        }

        public override void Update(float time)
        {
            // Обновление уже происходит в PhysicsWorld.Update
        }

        public void ApplyForce(Vector3 force)
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                _rigidBody.ApplyCentralForce(new BulletSharp.Math.Vector3(force.X, force.Y, force.Z));
            }
        }

        public void ApplyImpulse(Vector3 impulse)
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                _rigidBody.ApplyCentralImpulse(new BulletSharp.Math.Vector3(impulse.X, impulse.Y, impulse.Z));
            }
        }

        public void SetVelocity(Vector3 velocity)
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                _rigidBody.LinearVelocity = new BulletSharp.Math.Vector3(velocity.X, velocity.Y, velocity.Z);
            }
        }

        public void SetAngularVelocity(Vector3 angularVelocity)
        {
            if (_rigidBody != null && !_rigidBody.IsStaticObject && !_isKinematic)
            {
                _rigidBody.AngularVelocity = new BulletSharp.Math.Vector3(angularVelocity.X, angularVelocity.Y, angularVelocity.Z);
            }
        }
    }
}