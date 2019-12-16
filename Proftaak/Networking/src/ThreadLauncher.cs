using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking
{
    /// <summary>
    /// A worker thread for received packets
    /// </summary>
    public class ThreadLauncher
    {
        /// <summary>
        /// A callback for the worker thread
        /// </summary>
        /// <param name="connection">The connection a packet was received on</param>
        /// <param name="data">The data that was received</param>
        public delegate void OnPacket(Connection connection, byte[] data);

        private readonly Queue<byte[]> _packets = new Queue<byte[]>();
        private OnPacket _callback;
        private readonly Connection _parent;

        /// <param name="connection">The connection you want to assign this thread to</param>
        /// <param name="callback">The callback you want to get dispatched when a packet gets received</param>
        public ThreadLauncher(Connection connection, OnPacket callback)
        {
            _callback = callback;
            _parent = connection;
        }

        /// <param name="callback">The callback you want to get dispatched when a packet gets received</param>
        public void SetCallback(OnPacket callback)
        {
            _callback = callback;
        }

        /// <summary>
        /// Request the worker thread to execute a callback
        /// </summary>
        /// <param name="packet">The received data</param>
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
