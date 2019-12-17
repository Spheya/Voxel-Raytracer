using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        //Class1 test = new Class1();
        static void Main(string[] args)
        {
            //new Program();
            Console.WriteLine("2");
            UdpClient udpClient = new UdpClient();
            ASCIIEncoding ascii = new ASCIIEncoding();
            while (true)
            {
                try
                {
                    Console.WriteLine("bezig");
                    String bericht = "Test";
                    Int32 port = 13000;
                    String server = "127.0.0.1";
                    udpClient.Connect(server, port);
                    Byte[] encodedBytes = ascii.GetBytes(bericht);
                    udpClient.Send(encodedBytes, encodedBytes.Length);
                    Thread.Sleep(1000);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
    }
}
