using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking
{
    public class ThreadLauncher
    {
        public delegate void OnPacket(Connection connection, byte[] data);

        private readonly Queue<byte[]> _packets = new Queue<byte[]>();
        private OnPacket _callback;
        private readonly Connection _parent;

        public ThreadLauncher(Connection connection, OnPacket callback)
        {
            _callback = callback;
            _parent = connection;
        }

        public void SetCallback(OnPacket callback)
        {
            _callback = callback;
        }

        public void Request(byte[] packet)
        {
            bool threadRunning = false;
            lock (_packets)
            {
                if (_packets.Count > 0)
                    threadRunning = true;

                _packets.Enqueue(packet);
            }

            if(!threadRunning)
                StartThread(_packets, _callback, _parent);
        }

        private static void StartThread(Queue<byte[]> packets, OnPacket callback, Connection connection)
        {
            new Thread(() =>
            {
                byte[] packet = null;
                while (DoStuff(packets, ref packet))
                {
                    callback(connection, packet);
                }

            }).Start();
        }

        private static bool DoStuff(Queue<byte[]> packets, ref byte[] packet)
        {
            lock (packets)
            {
                if (packets.Count > 0)
                {
                    packet = packets.Dequeue();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
