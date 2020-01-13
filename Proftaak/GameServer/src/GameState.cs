using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking;

namespace GameServer
{
    class GameState : ApplicationState
    {
        public override void ProcessPacket(IConnection sender, byte[] data)
        {
            throw new NotImplementedException();
        }

        public override void Update(float deltatime)
        {
            throw new NotImplementedException();
        }
    }
}
