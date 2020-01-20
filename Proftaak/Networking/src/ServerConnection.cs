using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Networking
{
    public class ConnectionEventArgs : EventArgs
    {
        public IConnection Socket { get; set; }
    }

    public class ServerConnection
    {
        public delegate IConnection CreateConnection();
        public event EventHandler<ConnectionEventArgs> OnConnection;

        private readonly Thread _thread;

        public ServerConnection(CreateConnection creator)
        {
            _thread = new Thread(() =>
            {
                while (true)
                {
                    IConnection connection = creator();

                    OnConnection?.Invoke(this, new ConnectionEventArgs { Socket = connection });
                }
            });
            _thread.Start();
        }

        public void Close()
        {
            _thread.Abort();
        }
    }
}
