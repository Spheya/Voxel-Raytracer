using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

using Networking;

namespace GameServer
{
    class Server
    {
        private ApplicationState _applicationState = new GameState();

        private Dictionary<IConnection, Client> _clients = new Dictionary<IConnection, Client>();

        private readonly PacketSender _packetSender = new PacketSender();
        private readonly ServerConnection _connection;

        private readonly Thread _updateLoop;

        public Server(int port)
        {
            Console.WriteLine("Starting server at port: " + port);

            _connection = new ServerConnection(() => TcpConnection.Listen(port, OnPacketReceived));
            _connection.OnConnection += OnConnect;

            _applicationState.BroadCaster = _packetSender;
            _applicationState.Clients = _clients;
            _applicationState.OnCreate();

            _updateLoop = new Thread(() =>
            {
                while (true)
                {
                    lock (_packetSender)
                    {
                        lock (_applicationState)
                        {
                            _applicationState.Update(0.01f);

                            if (_applicationState.IsStateRequested())
                            {
                                _applicationState.OnDestroy();
                                _applicationState = _applicationState.GetRequestedState();
                                _applicationState.BroadCaster = _packetSender;
                                _applicationState.Clients = _clients;
                                _applicationState.OnCreate();
                            }
                        }


                        _packetSender.Send();

                        Thread.Sleep(100);
                    }
                }
            });
            _updateLoop.Start();

            Console.WriteLine("Server started!");
        }

        private void OnPacketReceived(IConnection connection, byte[] data)
        {
            // Validate packet

            // Update world

            // Enqueue update
            _packetSender.EnqueuePacket(data);
        }

        private void OnConnect(object sender, ConnectionEventArgs args)
        {
            for(ulong i = 1; true; i++)
            {
                if (!_clients.ContainsKey(args.Socket))
                {
                    _clients.Add(args.Socket, new Client() { Id = i, Connection = args.Socket });

                    args.Socket.Send(new byte[] { 1 }.Concat(BitConverter.GetBytes(i)).ToArray()); // Send Id packet

                    lock (_applicationState)
                        _applicationState.OnConnect(args.Socket, i);

                    break;
                }
            }

            _packetSender.AddReceiver(args.Socket);
        }

        public void Close()
        {
            _updateLoop.Abort();
            _connection.Close();
        }
    }
}
