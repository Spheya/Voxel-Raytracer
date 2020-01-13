using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking;
using System.Diagnostics;

namespace GameServer
{
    abstract class ApplicationState
    {
        Dictionary<ulong, Client> Clients { get; set; }
        PacketSender BroadCaster { get; set; }

        public abstract void OnCreate();
        public abstract void Update(float deltatime);
        public abstract void ProcessPacket(IConnection sender, byte[] data);
        public abstract void OnDestroy();

        protected void RequestState(ApplicationState state)
        {
            _requestedState = state;
        }

        public bool IsStateRequested()
        {
            return _requestedState != null;
        }

        public ApplicationState GetRequestedState()
        {
            Debug.Assert(_requestedState != null);
            return _requestedState;
        }


        private ApplicationState _requestedState = null;
    }
}
