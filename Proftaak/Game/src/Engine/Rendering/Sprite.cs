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
        public Transform Transform { get; set; }
        public Colour Colour { get; set; }
        public Texture Texture { get; set; }

        public Sprite(Texture texture, Transform transform, Colour colour)
        {
            Texture = texture;
            Transform = transform;
            Colour = colour;
        }
        public Sprite(Texture texture, Transform transform) :
            this(texture, transform, new Colour(1.0f, 1.0f, 1.0f))
        { }

        public Sprite (Texture texture) :
            this(texture, new Transform(Vector3.Zero, Vector3.Zero, new Vector3(32.0f)))
        { }
    }
}
