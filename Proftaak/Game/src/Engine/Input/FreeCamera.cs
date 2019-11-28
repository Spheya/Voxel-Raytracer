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
            //Mike, je kunt gebruik maken van KeyboardInput om te kijken welke toetsen ingedrukt worden.
            //Voorbeeld:
            //bool pressed = KeyboardInput.IsForwardDown();
            //Dit is voor horizontale bewegingen. Deze functies staan in KeyboardInput.cs. Als je hier meer aan toe wilt voegen, dan moet je gewoon een kopie maken die functies met een andere naam en een andere toets.
            //Voor correcte beweging moet je de hoeveelheid die je verplaatst in units per seconden zetten (oftewel meters per seconde) en vermenigvuldigen met de deltatime.
            //Vermenigvuldigen met de deltatime zorgt ervoor dat je niet sneller verplaatst als het programma sneller werkt.
        }
    }
}
