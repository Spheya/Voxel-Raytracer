using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Maths;

namespace Game.Engine.Rendering
{
    class Camera
    {
        private readonly Transform _transform = new Transform();

        public Vector3 Position => _transform.Position;
        public Vector3 Rotation => _transform.Rotation;

        public Camera()
        {}

        public Camera(Vector3 position, Vector3 rotation)
        {
            _transform = new Transform(position, rotation, new Vector3(1.0f, 1.0f, 1.0f));
        }

        public Matrix4 CalculateMatrix()
        {
            return _transform.CalculateMatrix();
        }

        public Matrix4 CalculateViewMatrix()
        {
            return _transform.CalculateInverseMatrix();
        }
    }
}
