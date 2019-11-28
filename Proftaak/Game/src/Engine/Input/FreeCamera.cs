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
        private float _speed = 3f; //Speed of the camera
        private float _turnSpeed = 1f; //Turning speed of the camera

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
            //Mike, je kunt gebruik maken van KeyboardInput om te kijken welke toetsen ingedrukt worden.
            //Voorbeeld:

            bool pressedFD = KeyboardInput.IsForwardDown();
            bool pressedBD = KeyboardInput.IsBackwardDown();
            bool pressedSLD = KeyboardInput.IsStrafeLeftDown();
            bool pressedSRD = KeyboardInput.IsStrafeRightDown();

            bool pressedLeft = KeyboardInput.IsTurnLeftDown();

            if (pressedFD == true)
            {
                _transform.Position += new Vector3(_speed * (float)Math.Sin(_transform.Rotation.Y) * deltatime, 0f, _speed * (float)Math.Cos(_transform.Rotation.Y) * deltatime);
            }
            else if (pressedBD == true)
            {
                _transform.Position += new Vector3(1 * deltatime, 0f, 0f);
            }
            else if (pressedSLD == true)
            {
                _transform.Position += new Vector3(1 * deltatime, 0f, 0f);
            }
            else if (pressedSRD == true)
            {
                _transform.Position += new Vector3(1 * deltatime, 0f, 0f);
            }

            if (pressedLeft)
            {
                _transform.Rotation += new Vector3(0f, _turnSpeed * deltatime, 0f);
            }
            //Dit is voor horizontale bewegingen. Deze functies staan in KeyboardInput.cs. Als je hier meer aan toe wilt voegen, dan moet je gewoon een kopie maken die functies met een andere naam en een andere toets.
            //Voor correcte beweging moet je de hoeveelheid die je verplaatst in units per seconden zetten (oftewel meters per seconde) en vermenigvuldigen met de deltatime.
            //Vermenigvuldigen met de deltatime zorgt ervoor dat je niet sneller verplaatst als het programma sneller werkt.
        }
    }
}
