using System;
using System.Net;
using System.Net.Sockets;

namespace Networking
{
    /// <summary>
    /// A wrapper around a connection socket, works for both the client and the server
    /// </summary>
    public sealed class TcpConnection : IConnection
    {
        private struct PacketData
        {
            public bool searching;
            public byte[] data;
            public readonly Socket socket;
            public readonly ThreadLauncher threadLauncher;

            public PacketData(bool searching, byte[] data, Socket socket, ThreadLauncher threadLauncher)
            {
                this.searching = searching;
                this.data = data;
                this.socket = socket;
                this.threadLauncher = threadLauncher;
            }
        }

        private readonly Socket _socket;
        private readonly ThreadLauncher _launcher;


        public ThreadLauncher.OnPacket Callback { set => _launcher.SetCallback(value); }

        /// <param name="target">The IP you want to connect to</param>
        /// <param name="port">The port you want to connect to</param>
        /// <param name="callback">The callback you want to get dispatched when a packet gets received</param>
        public TcpConnection(IPAddress target, int port, ThreadLauncher.OnPacket callback)
        {
            _launcher = new ThreadLauncher(this, callback);

            // Setup a TCP TcpConnection
            byte[] buffer = new byte[1];

            _socket = new Socket(target.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //_socket.Connect(new IPEndPoint(target, port));
            //_socket.BeginReceive(buffer, 0, 1, 0, AsyncReceiveCallback, new PacketData(true, buffer, _socket, _launcher));
        }

        private TcpConnection(Socket socket, ThreadLauncher.OnPacket callback)
        {
            var threadLauncher = new ThreadLauncher(this, callback);

            byte[] buffer = new byte[1];

            _socket = socket;
            _socket.BeginReceive(buffer, 0, 1, 0, AsyncReceiveCallback, new PacketData(true, buffer, _socket, threadLauncher));
        }

        /// <summary>
        /// Open a port and wait for someone to connect
        /// </summary>
        /// <param name="port">The port you want to open</param>
        /// <param name="callback">The callback you want to get dispatched when a packet gets received</param>
        /// <returns></returns>
        public static IConnection Listen(int port, ThreadLauncher.OnPacket callback)
        {
            var localEndPoint = new IPEndPoint(IPAddress.Any, port);
            var listener = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            Socket socket;

            // Setup the listener socket
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                // Accept the first connection
                socket = listener.Accept();
            }
            finally
            {
                listener.Close();
            }

            return new TcpConnection(socket, callback);
        }

        /// <summary>
        /// Send some data over the socket
        /// </summary>
        /// <param name="data">The data you want the other side to reveive</param>
        public void Send(byte[] data)
        {
            _socket.Send(new[] { (byte)data.Length }, 0, 1, 0);
            _socket.Send(data, 0, data.Length, 0);
        }

        /// <summary>
        /// Close the connection
        /// </summary>
        public void Close()
        {
            _socket.Close();
        }

        private static void AsyncReceiveCallback(IAsyncResult ar)
        {
            PacketData state = (PacketData)ar.AsyncState;
            if (state.searching)
            {
                // Received the length of the packet
                int packetSize = state.data[0];

                // Prepare to receive content
                state.data = new byte[packetSize];
                state.searching = false;

                try
                {
                    state.socket.BeginReceive(state.data, 0, packetSize, 0, AsyncReceiveCallback, state);
                }
                catch (Exception)
                {
                    //Console.WriteLine("Player disconnected for mysterious reasons!");
                }
            }
            else
            {
                // Received the content of the packet
                state.threadLauncher.Request(state.data);

                // Prepare to search for a new packet
                state.data = new byte[1];
                state.searching = true;

                try
                {
                    state.socket.BeginReceive(state.data, 0, 1, 0, AsyncReceiveCallback, state);
                }
                catch (Exception)
                {
                    //Console.WriteLine("Player disconnected for mysterious reasons!");
                }
            }
        }
    }
}
