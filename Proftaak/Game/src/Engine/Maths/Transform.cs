using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Game.Engine.Maths
{
    class Transform
    {
        public Vector3 Position { get; set; } = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 Rotation { get; set; } = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 Scale { get; set; } = new Vector3(1.0f, 1.0f, 1.0f);

        public Transform()
        {}

        public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Matrix4 CalculateMatrix()
        {
            return Matrix4.CreateScale(Scale)
                   * Matrix4.CreateTranslation(-Scale * 0.5f)
                   * Matrix4.CreateRotationX(Rotation.X)
                   * Matrix4.CreateRotationY(Rotation.Y)
                   * Matrix4.CreateRotationZ(Rotation.Z)
                   * Matrix4.CreateTranslation(Position + Scale * 0.5f);
        }

        public Matrix4 CalculateInverseMatrix()
        {
            return Matrix4.CreateRotationZ(-Rotation.Z)
                   * Matrix4.CreateRotationY(-Rotation.Y)
                   * Matrix4.CreateRotationX(-Rotation.X)
                   * Matrix4.CreateScale(new Vector3(1.0f / Scale.X, 1.0f / Scale.Y, 1.0f / Scale.Z))
                   * Matrix4.CreateTranslation(-Position);
        }

        public Matrix4 CalculateNormalMatrix()
        {
            return Matrix4.CreateScale(1.0f / Scale.X, 1.0f / Scale.Y, 1.0f / Scale.Z)
                   * Matrix4.CreateTranslation(-Scale * 0.5f)
                   * Matrix4.CreateRotationX(Rotation.X)
                   * Matrix4.CreateRotationY(Rotation.Y)
                   * Matrix4.CreateRotationZ(Rotation.Z)
                   * Matrix4.CreateTranslation(Position + Scale * 0.5f);
        }

    }
}
