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
        /// <summary>
        /// The near plane
        /// </summary>
        public float Near { get; set; } = 0.1f;

        /// <summary>
        /// The far plane
        /// </summary>
        public float Far { get; set; } = 100.0f;

        /// <summary>
        /// The vertical field of view
        /// </summary>
        public float Fov { get; set; } = 90.0f;

        public Transform _transform = new Transform();

        /// <summary>
        /// The position of the camera
        /// </summary>
        public Vector3 Position => _transform.Position;

        /// <summary>
        /// The rotation of the camera
        /// </summary>
        public Vector3 Rotation => _transform.Rotation;

        public Camera()
        {}

        /// <param name="position">Initial position of the camera</param>
        /// <param name="rotation">Initial rotation of the camera</param>
        public Camera(Vector3 position, Vector3 rotation)
        {
            _transform = new Transform(position, rotation, new Vector3(1.0f, 1.0f, 1.0f));
        }

        /// <summary>
        /// Calculates a matrix to go from camera space to world space
        /// </summary>
        /// <returns>The transformation matrix of the camera</returns>
        public Matrix4 CalculateMatrix()
        {
            return _transform.CalculateMatrix();
        }

        /// <summary>
        /// Calculates a matrix to go from world space to camera space
        /// </summary>
        /// <returns>The view matrix of the camera</returns>
        public Matrix4 CalculateViewMatrix()
        {
            return _transform.CalculateInverseMatrix();
        }

        /// <summary>
        /// Calculates a matrix to go from camera space to screen space
        /// </summary>
        /// <param name="window">The window you want to use the projection matrix on</param>
        /// <returns>The projection matrix for the given window and the camera</returns>
        public Matrix4 CalculateProjectionMatrix(GameWindow window)
        {
            float aspect = window.Width / (float)window.Height;
            return Matrix4.CreatePerspectiveFieldOfView(Fov * (float)(Math.PI / 180.0f), aspect, Near, Far);
        }

        public virtual void Update(float deltatime) { }
    }
}
