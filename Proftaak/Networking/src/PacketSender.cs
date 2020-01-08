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
        private List<IConnection> _receivers;
        private Queue<byte[]> _packetQueue;

        public void AddReceiver(IConnection connection) => _receivers.Add(connection);
        public bool RemoveReceiver(IConnection connection) => _receivers.Remove(connection);

        public void EnqueuePacket(byte[] packet) => _packetQueue.Enqueue(packet);

        public void Send()
        {
            while(_packetQueue.Count != 0)
            {
                byte[] packet = _packetQueue.Dequeue();

                foreach (var receiver in _receivers)
                    receiver.Send(packet);
            }
        }
    }
}
