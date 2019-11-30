using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Maths;
using OpenTK;

namespace Game.Engine.Rendering
{
    class Sprite
    {
        public Transform Transform { get; set; } = new Transform(Vector3.Zero, Vector3.Zero, new Vector3(32.0f));
        public Colour Colour { get; set; } = new Colour(1.0f, 1.0f, 1.0f);
    }
}
