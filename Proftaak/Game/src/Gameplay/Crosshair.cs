using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Rendering;

namespace Game.Gameplay
{
    class Crosshair
    {
        private Sprite[] _edges = new Sprite[4];

        public float Growth { get; set; } = 32.0f;

        public Crosshair()
        {
            Texture edgeText = new Texture(@"res\");

            for (int i = 0; i < 4; i++)
            {
                //_edges[i] = new Sprite();
            }
        }

    }
}
