using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Networking;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server(42069);
        }
    }
}
