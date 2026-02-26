using System.Drawing;
using OpenTK.Mathematics;

namespace OpenTKProject
{
    public class Light
    {
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }

        public Light(Vector3 position, Color color)
        {
            Position = position;
            Color = color;
            Ambient = new Vector3(0.2f, 0.2f, 0.2f);
            Diffuse = new Vector3(0.5f, 0.5f, 0.5f);
            Specular = new Vector3(1.0f, 1.0f, 1.0f);
        }

        public Matrix4 GetLightSpaceMatrix(float size = 10.0f, float nearPlane = 1.0f, float farPlane = 10.0f)
        {
            Matrix4 lightProjection = Matrix4.CreateOrthographicOffCenter(
                -size, size, -size, size, nearPlane, farPlane);
            Matrix4 lightView = Matrix4.LookAt(Position, Vector3.Zero, Vector3.UnitY);
            return lightView * lightProjection;
        }

        public Vector3 ColorToVec3()
        {
            return new Vector3(Color.R / 100f, Color.G / 100f, Color.B / 100f);
        }
    }
}