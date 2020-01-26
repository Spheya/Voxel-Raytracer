using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoInput;
using InputManager;

namespace ArduinoInput
{
    using System.IO.Ports;
    class Program
    {

        static SerialPort SP;
        

        static void Controller()
        {
            string buffer = "";
            while (SP.BytesToRead > 0)
            {
                buffer = buffer + (char)SP.ReadByte();
            }
            while (buffer.Contains("\n"))
            {
                string toPrint = SP.ReadLine();
                toPrint = buffer.Substring(0, buffer.IndexOf('\n'));
                buffer = buffer.Substring(buffer.IndexOf('\n') + 1);
                Console.WriteLine(DateTime.Now.ToString() + " : " + toPrint);
                JoystickEen joy = new JoystickEen();
                JoystickTwee joyTwee = new JoystickTwee();
                Buttons knoppie = new Buttons();
                knoppie.Knop = "";
                joy.Knop = "";
                SP.Write("AAN");
                Console.WriteLine("AAN");
                while (SP.BytesToRead > 0) 
                {
                    Console.Write((char) SP.ReadChar());
                }
                if (toPrint.Contains("Een"))
                {
                    knoppie.Knop = "Een";
                }
                if (toPrint.Contains("Twee"))
                {
                    knoppie.Knop = "Twee";
                }
                if (toPrint.Contains("Drie"))
                {
                    knoppie.Knop = "Drie";
                }
                if (toPrint.Contains("Vier"))
                {
                    knoppie.Knop = "Vier";
                }
                if (toPrint.Contains("joystickButton"))
                {
                    joy.Knop = "joystickButton";
                }
                if (toPrint.Contains("W"))
                {
                    joy.Knop = "W";
                }
                if (toPrint.Contains("A"))
                {
                    joy.Knop = "A";
                }
                if (toPrint.Contains("S"))
                {
                    joy.Knop = "S";
                }
                if (toPrint.Contains("D"))
                {
                    joy.Knop = "D";
                }
                if (toPrint.Contains("joyTwee"))
                {
                    joyTwee.Knop = "Joytwee";
                }
                if (toPrint.Contains("Rechts"))
                {
                    joyTwee.Knop = "Rechts";
                }
                if (toPrint.Contains("Links"))
                {
                    joyTwee.Knop = "Links";
                }
                if (toPrint.Contains("Voor"))
                {
                    joyTwee.Knop = "Voor";
                }
                if (toPrint.Contains("Achter"))
                {
                    joyTwee.Knop = "Achter";
                }
                if (toPrint.Contains("YStop"))
                {
                    joy.Knop = "YStop";
                }
                if (toPrint.Contains("XStop"))
                {
                    joy.Knop = "XStop";
                }
                if (toPrint.Contains("jsStop"))
                {
                    joy.Knop = "jsStop";
                }
                if (toPrint.Contains("j2Stop"))
                {
                    joyTwee.Knop = "j2Stop";
                }
                if (toPrint.Contains("XStopTwee"))
                {
                    joyTwee.Knop = "XStopTwee";
                }
                if (toPrint.Contains("YStopTwee"))
                {
                    joyTwee.Knop = "YStopTwee";
                }
                if (toPrint.Contains("BtnStop"))
                {
                    knoppie.Knop = "BtnStop";
                }

                joy.JoyStick1();
                joyTwee.Joystick2();
                knoppie.Button();
                
            }
        }

        static void Main(string[] args)
        {
           
            SP = new SerialPort();
            SP.PortName = "COM3";
            SP.BaudRate = 9600;
            SP.Open();
            while (true)
            {
                Controller();
            }            
        }
    }
}
