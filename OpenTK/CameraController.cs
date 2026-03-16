using OpenTK.Mathematics;

namespace OpenTKProject
{
    public enum CameraMode
    {
        Free,        // Свободный режим
        Orbit        // Вращение вокруг объекта
    }

    public class CameraController
    {
        private float _speed = 1.5f;
        private float _zoomSpeed = 2.0f;
        private CameraMode _currentMode = CameraMode.Free;
        private SceneObject _targetObject = null;
        private float _orbitDistance = 0.5f;
        private float _minDistance = 0.5f;
        private float _maxDistance = 10.0f;

        public Vector3 Position { get; private set; }
        public Vector3 Front { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }
        public float Yaw { get; private set; } = -90.0f;
        public float Pitch { get; private set; } = 0.0f;
        public float MouseSensitivity { get; set; } = 0.1f;
        public CameraMode CurrentMode => _currentMode;

        public CameraController(float speed, Vector3 position)
        {
            _speed = speed;
            Position = position;
            UpdateVectors();
        }

        public void MoveForward(float deltaTime) => Position += Front * _speed * deltaTime;
        public void MoveBackward(float deltaTime) => Position -= Front * _speed * deltaTime;
        public void MoveLeft(float deltaTime) => Position -= Right * _speed * deltaTime;
        public void MoveRight(float deltaTime) => Position += Right * _speed * deltaTime;
        public void MoveUp(float deltaTime) => Position += Up * _speed * deltaTime;
        public void MoveDown(float deltaTime) => Position -= Up * _speed * deltaTime;

        public void Zoom(float delta)
        {
            if (_currentMode == CameraMode.Free)
            {
                _speed += delta * 0.1f;
                _speed = MathHelper.Clamp(_speed, 0.5f, 20.0f);
            }
            else if (_currentMode == CameraMode.Orbit && _targetObject != null)
            {
                _orbitDistance -= delta * _zoomSpeed * 0.1f;
                _orbitDistance = MathHelper.Clamp(_orbitDistance, _minDistance, _maxDistance);
                UpdateOrbitPosition();
            }
        }

        public Matrix4 GetViewMatrix()
        {
            if (_currentMode == CameraMode.Orbit && _targetObject != null)
            {
                UpdateOrbitPosition();
                return Matrix4.LookAt(Position, _targetObject.Position, Up);
            }
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        public void RotateCamera(float xOffset, float yOffset)
        {
            xOffset *= MouseSensitivity;
            yOffset *= MouseSensitivity;

            Yaw += xOffset;
            Pitch += yOffset;
            Pitch = MathHelper.Clamp(Pitch, -89.0f, 89.0f);

            if (_currentMode == CameraMode.Free)
                UpdateVectors();
            else if (_currentMode == CameraMode.Orbit && _targetObject != null)
                UpdateOrbitPosition();
        }

        private void UpdateVectors()
        {
            Front = Vector3.Normalize(new Vector3(
                MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch)),
                MathF.Sin(MathHelper.DegreesToRadians(Pitch)),
                MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch))
            ));
            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }

        private void UpdateOrbitPosition()
        {
            if (_targetObject == null) return;

            Position = _targetObject.Position + new Vector3(
                MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch)) * _orbitDistance,
                -MathF.Sin(MathHelper.DegreesToRadians(Pitch)) * _orbitDistance,
                MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch)) * _orbitDistance
            );

            Front = Vector3.Normalize(_targetObject.Position - Position);
            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }

        public void SetFreeMode() => _currentMode = CameraMode.Free;

        public void SetOrbitMode(SceneObject target, float distance = 5.0f)
        {
            if (target == null) return;
            _currentMode = CameraMode.Orbit;
            _targetObject = target;
            _orbitDistance = MathHelper.Clamp(distance, _minDistance, _maxDistance);
            UpdateOrbitPosition();
        }

        public void SetDistanceLimits(float min, float max)
        {
            _minDistance = min;
            _maxDistance = max;
        }
    }
}