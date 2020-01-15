using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Networking;

namespace GameServer
{
    class Client
    {
        public ulong Id { get; set; }
        public IConnection Connection { get; set; }
    }
}
