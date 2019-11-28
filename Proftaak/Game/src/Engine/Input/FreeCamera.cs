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
            DateTime time1 = DateTime.Now;
            DateTime time2 = DateTime.Now;
            float deltaTime = (time2.Ticks - time1.Ticks);
            int deltaTime2 = (int)deltaTime;
            //Mike, je kunt gebruik maken van KeyboardInput om te kijken welke toetsen ingedrukt worden.
            //Voorbeeld:

            bool pressedFD = KeyboardInput.IsForwardDown();
            bool pressedBD = KeyboardInput.IsBackwardDown();
            bool pressedSLD = KeyboardInput.IsStrafeLeftDown();
            bool pressedSRD = KeyboardInput.IsStrafeRightDown();

            if (pressedFD == true)
            {
                _transform.Position += new Vector3(1*deltaTime2, 0f, 0f);
            }
            else if (pressedBD == true)
            {
                Position += 1*deltaTime2;
            }
            else if (pressedSLD == true)
            {
                position += 1*deltaTime2;
            }
            else if (pressedSRD == true)
            {
                position += 1*deltaTime2;
            }
            //Dit is voor horizontale bewegingen. Deze functies staan in KeyboardInput.cs. Als je hier meer aan toe wilt voegen, dan moet je gewoon een kopie maken die functies met een andere naam en een andere toets.
            //Voor correcte beweging moet je de hoeveelheid die je verplaatst in units per seconden zetten (oftewel meters per seconde) en vermenigvuldigen met de deltatime.
            //Vermenigvuldigen met de deltatime zorgt ervoor dat je niet sneller verplaatst als het programma sneller werkt.
        }
    }
}
