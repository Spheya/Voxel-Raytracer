using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


                if (toPrint.Contains("Een"))
                {

                }
                if (toPrint.Contains("Twee"))
                {

                }
                if (toPrint.Contains("Drie"))
                {

                }
                if (toPrint.Contains("Vier"))
                {

                }
                if (toPrint.Contains("joystickButton"))
                {
                    
                }
                if (toPrint.Contains("W"))
                {
                    
                }
                if (toPrint.Contains("A"))
                {

                }
                if (toPrint.Contains("S"))
                {

                }
                if (toPrint.Contains("D"))
                {

                }
            }
        }

        static void Main(string[] args)
        {
           
            SP = new SerialPort();
            SP.PortName = "COM3";
            SP.BaudRate = 9600;
            //SP.ReadTimeout = 500;
            SP.Open();
            while (true)
            {
                Controller();
            }

        }
    }
}
