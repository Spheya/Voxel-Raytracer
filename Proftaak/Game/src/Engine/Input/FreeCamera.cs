using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Rendering;
using OpenTK;

namespace Game.Engine.Input
{
    sealed class FreeCamera : Camera
    {
        private static float mouseSensitivity = 0.001f;

        private float _speed = 10f; //Speed of the camera

        /// <summary>
        /// Calls the Camera constructor
        /// </summary>
        /// <param name="position">Location of the camera</param>
        /// <param name="rotation">Rotation vector of the camera</param>
        public FreeCamera(Vector3 position, Vector3 rotation) : base(position, rotation)
        {}

        /// <summary>
        /// Updates the camera position using the user input
        /// </summary>
        /// <param name="deltatime">The time (in seconds) the previous frame took</param>
        public void Update(float deltatime)
        {
            bool pressedFD = KeyboardInput.IsForwardDown();
            bool pressedBD = KeyboardInput.IsBackwardDown();
            bool pressedSLD = KeyboardInput.IsStrafeLeftDown();
            bool pressedSRD = KeyboardInput.IsStrafeRightDown();

            if (pressedFD == true)
            {
                _transform.Position += new Vector3(_speed * (float)Math.Sin(_transform.Rotation.Y) * deltatime, 0f, _speed * (float)Math.Cos(_transform.Rotation.Y) * deltatime);
            }
            if (pressedBD == true)
            {
                //_transform.Position += new Vector3(1 * deltatime, 0f, 0f);
                _transform.Position -= new Vector3(_speed * (float)Math.Sin(_transform.Rotation.Y) * deltatime, 0f, _speed * (float)Math.Cos(_transform.Rotation.Y) * deltatime);
            }
            if (pressedSLD == true)
            {
                //_transform.Position += new Vector3(1 * deltatime, 0f, 0f);
                _transform.Position -= new Vector3(_speed * (float)Math.Cos(-_transform.Rotation.Y) * deltatime, 0f, _speed * (float)Math.Sin(-_transform.Rotation.Y) * deltatime);
            }
            if (pressedSRD == true)
            {
                //_transform.Position += new Vector3(1 * deltatime, 0f, 0f);
                _transform.Position += new Vector3(_speed * (float)Math.Cos(-_transform.Rotation.Y) * deltatime, 0f, _speed * (float)Math.Sin(-_transform.Rotation.Y) * deltatime);
            }

            if (KeyboardInput.IsDownDown())
            {
                _transform.Position += new Vector3(0.0f, -_speed * deltatime, 0.0f);
            }
            if (KeyboardInput.IsUpDown())
            {
                _transform.Position += new Vector3(0.0f, _speed * deltatime, 0.0f);
            }

            Vector2 delta  = MouseInput.GetMouseDelta() * mouseSensitivity;
            _transform.Rotation += new Vector3(delta.Y, delta.X, 0.0f);


        }
    }
}
