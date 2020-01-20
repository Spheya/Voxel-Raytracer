using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InputManager;

namespace ArduinoInput
{
    class Buttons
    {
        public string Knop
        {
            get; set;
        }
        public void Reset()
        {
            Keyboard.KeyUp(Keys.Q);
            Keyboard.KeyUp(Keys.E);
            Keyboard.KeyUp(Keys.Z);
            Keyboard.KeyUp(Keys.X);
        }

        public void Button()
        {
            if (Knop == "Een")
            {
                Keyboard.KeyDown(Keys.Q);
            }
            if (Knop == "Twee")
            {
                Keyboard.KeyDown(Keys.E);
            }
            if (Knop == "Drie")
            {
                Keyboard.KeyDown(Keys.Z);
            }
            if (Knop == "Vier")
            {
                Keyboard.KeyDown(Keys.X);
            }
            if (Knop == "BtnStop")
            {
                Reset();
            }
        }
    }
}
