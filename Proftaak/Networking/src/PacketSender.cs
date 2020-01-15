using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntitySystem;

namespace Networking
{
    public class PacketSender
    {
        private readonly List<IConnection> _receivers = new List<IConnection>();
        private readonly Queue<byte[]> _packetQueue = new Queue<byte[]>();

        public event EventHandler<ConnectionEventArgs> OnDisconnect;

        public void AddReceiver(IConnection connection) => _receivers.Add(connection);
        public bool RemoveReceiver(IConnection connection) => _receivers.Remove(connection);

        public void EnqueuePacket(byte[] packet) => _packetQueue.Enqueue(packet);

        public void Send()
        {
            while(_packetQueue.Count != 0)
            {
                byte[] packet = _packetQueue.Dequeue();

                for (int i = 0; i < _receivers.Count; i++)
                {
                    var receiver = _receivers[i];

                    try
                    {
                        receiver.Send(packet);
                    }
                    catch (Exception)
                    {
                        OnDisconnect?.Invoke(this, new ConnectionEventArgs() { Socket = receiver });
                        _receivers.RemoveAt(i);
                        --i;
                    }
                }
            }
        }
    }
}
