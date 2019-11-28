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

        }
    }
}
