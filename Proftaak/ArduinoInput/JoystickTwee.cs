using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InputManager;
using System.Windows.Forms;

namespace ArduinoInput
{
    class JoystickTwee
    {
        public string Knop
        {
            get; set;
        }

        public void ResetJ()
        {
            Keyboard.KeyUp(Keys.L);
        }
        public void ResetX()
        {
            Keyboard.KeyUp(Keys.Left);
            Keyboard.KeyUp(Keys.Right);
        }
        public void ResetY()
        {
            Keyboard.KeyUp(Keys.Up);
            Keyboard.KeyUp(Keys.Down);
        }

        public void Joystick2()
        {

            if (Knop == "Joytwee")
            {
                Keyboard.KeyDown(Keys.L);
            }
            if (Knop == "Rechts")
            {
                Keyboard.KeyDown(Keys.Right);
            }
            if (Knop == "Links")
            {
                Keyboard.KeyDown(Keys.Left);
            }
            if (Knop == "Boven")
            {
                Keyboard.KeyDown(Keys.Up);
            }
            if (Knop == "Onder")
            {
                Keyboard.KeyDown(Keys.Down);
            }
            if (Knop == "j2Stop")
            {
                ResetJ();
            }
            if (Knop == "XStopTwee")
            {
                ResetX();
            }
            if (Knop == "YStopTwee")
            {
                ResetY();
            }
        }
    }
}
