using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public sealed class UdpConnection : IConnection
    {
        private ThreadLauncher _threadlauncher;

        public ThreadLauncher.OnPacket Callback { set => _threadlauncher.SetCallback(value); }

        public UdpConnection(ThreadLauncher.OnPacket callback)
        {
            _threadlauncher = new ThreadLauncher(this, callback);
        }
        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
