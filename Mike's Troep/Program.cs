using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Program {

public class Class1
    {
            static void Main(string[] args) {
            //ConsoleApp1.Program test = new ConsoleApp1.Program();
            Console.WriteLine("1");
            int port = 13000;
            UdpClient receivingUdpClient = new UdpClient(port);
                while (true)
                {
                    try
                    {
                        //Console.WriteLine("hoi");
                        //var longVariable = (long)2130706531;
                        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                        //Console.WriteLine(ipAddress);
                        IPEndPoint RemoteIpEndPoint = new IPEndPoint(ipAddress, port);
                        Byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                        string returnData = Encoding.ASCII.GetString(receiveBytes);
                        Console.WriteLine("This is the message you received " + returnData.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }
    }