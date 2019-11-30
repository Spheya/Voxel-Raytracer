using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine.Rendering
{
    struct Colour
    {
        /// <summary>
        /// The red component of the colour
        /// range 0.0 - 1.0
        /// </summary>
        public float R { get; set; }

        /// <summary>
        /// The green component of the colour
        /// range 0.0 - 1.0
        /// </summary>
        public float G { get; set; }

        /// <summary>
        /// The blue component of the colour
        /// range 0.0 - 1.0
        /// </summary>
        public float B { get; set; }

        /// <summary>
        /// The alpha component of the colour
        /// range 0.0 - 1.0
        /// </summary>
        public float A { get; set; }

        /// <param name="r">The red component of the colour</param>
        /// <param name="g">The green component of the colour</param>
        /// <param name="b">The blue component of the colour</param>
        /// <param name="a">The alpha component of the colour</param>
        public Colour(float r, float g, float b, float a = 1.0f)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Lerps two colours together using a given factor
        /// </summary>
        /// <param name="col1">The first colour</param>
        /// <param name="col2">The second colour</param>
        /// <param name="factor">The percentage of the second colour that should be used for the mix</param>
        /// <returns>The mixed colour</returns>
        public static Colour Mix(Colour col1, Colour col2, float factor = 0.5f)
        {
            // Clamp factor between 0.0 and 1.0
            factor = Math.Min(Math.Max(factor, 0.0f), 1.0f);

            return new Colour(
                col1.R * (1.0f - factor) + col2.R * factor,
                col1.G * (1.0f - factor) + col2.G * factor,
                col1.B * (1.0f - factor) + col2.B * factor,
                col1.A * (1.0f - factor) + col2.A * factor
            );
        }
    }
}
