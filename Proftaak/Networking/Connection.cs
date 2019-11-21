using System;
using System.Net;
using System.Net.Sockets;

namespace Networking
{
    public class Connection
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

        public void SetCallback(ThreadLauncher.OnPacket callback)
        {
            _launcher.SetCallback(callback);
        }

        public Connection(IPAddress target, int port, ThreadLauncher.OnPacket callback)
        {
            _launcher = new ThreadLauncher(this, callback);

            // Setup a TCP Connection
            byte[] buffer = new byte[1];

            _socket = new Socket(target.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(new IPEndPoint(target, port));
            _socket.BeginReceive(buffer, 0, 1, 0, AsyncReceiveCallback, new PacketData(true, buffer, _socket, _launcher));
        }

        private Connection(Socket socket, ThreadLauncher.OnPacket callback)
        {
            var threadLauncher = new ThreadLauncher(this, callback);

            byte[] buffer = new byte[1];

            _socket = socket;
            _socket.BeginReceive(buffer, 0, 1, 0, AsyncReceiveCallback, new PacketData(true, buffer, _socket, threadLauncher));
        }

        public static Connection Listen(int port, ThreadLauncher.OnPacket callback)
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

            return new Connection(socket, callback);
        }

        public void Send(byte[] data)
        {
            _socket.Send(new[] { (byte)data.Length }, 0, 1, 0);
            _socket.Send(data, 0, data.Length, 0);
        }

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
