using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace Networking.src
{
    public class UDP
    {
        //Class1 test = new Class1();
        public static void dit()
        {
            //new Program();
            Console.WriteLine("2");
            UdpClient udpClient = new UdpClient();
            ASCIIEncoding ascii = new ASCIIEncoding();
            try
            {
                Console.WriteLine("bezig");
                String bericht = "haha";
                Int32 port = 13000;
                String server = "127.0.0.1";
                udpClient.Connect(server, port);
                Byte[] encodedBytes = ascii.GetBytes(bericht);
                udpClient.Send(encodedBytes, encodedBytes.Length);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
