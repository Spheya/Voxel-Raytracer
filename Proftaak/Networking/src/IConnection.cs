using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public interface IConnection
    {
        ThreadLauncher.OnPacket Callback { set; }

        void Send(byte[] data);
        void Close();
    }
}
