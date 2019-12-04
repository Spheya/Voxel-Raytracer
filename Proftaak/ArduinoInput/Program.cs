using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoInput
{
    class Program
    {
        public static void Test()
        {
            SerialPort port = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);

            try
            {
                port.Open();

                while (port.BytesToRead > 0)
                    Console.WriteLine(port.ReadByte());

            }
            finally
            {
                port.Close();
            }
        }

        static void Main(string[] args)
        {
        }
    }
}
