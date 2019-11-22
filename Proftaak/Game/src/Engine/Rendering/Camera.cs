using Abacus.SinglePrecision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.src.Engine.Rendering
{
    class Camera
    {
        public Vector3 Position;
        public Vector3 Forward;

        public Camera(Vector3 position, Vector3 forward)
        {
            Position = position;
            Forward = forward;
        }

        //TODO: Add helper functions here, like setting rotation and such
    }
}
