using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using ArduinoInput;
using System.Runtime.InteropServices;
using InputManager;
using System.Windows.Input;
using System.Windows.Forms;

namespace ArduinoInput
{

    public class JoystickEen
    {



        public string Knop
        {
            get; set;
        }

        public void ResetY()
        {
            Keyboard.KeyUp(Keys.W);
            Keyboard.KeyUp(Keys.S);
        }
        public void ResetX()
        {
            Keyboard.KeyUp(Keys.A);
            Keyboard.KeyUp(Keys.D);
        }
        public void ResetZ()
        {
            Keyboard.KeyUp(Keys.Space);
        }

        public void JoyStick1()
        {
            if (Knop == "joystickButton")
            {
                Keyboard.KeyDown(Keys.Space);
            }
            if (Knop == "W")
            {
                Keyboard.KeyDown(Keys.W);
            }
            if (Knop == "A")
            {
                Keyboard.KeyDown(Keys.A);
            }
            if (Knop == "S")
            {
                Keyboard.KeyDown(Keys.S);
            }
            if (Knop == "D")
            {
                Keyboard.KeyDown(Keys.D);
            }
            if (Knop == "YStop")
            {
                ResetY();
            }
            if (Knop == "XStop")
            {
                ResetX();
            }
            if (Knop == "jsStop")
            { 
                ResetZ();
            }
        }
    }
}
