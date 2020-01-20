using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Networking;
using EntitySystem;
using GameServer.Entities;

namespace GameServer
{
    class GameState : ApplicationState
    {
        private EntityManager _entityManager = new EntityManager();
        private EntityIdGenerator _idGenerator = new EntityIdGenerator();

        public override void OnConnect(IConnection connection, ulong userId)
        {
            _entityManager.Add(new Player(_idGenerator.Generate(), userId));
        }

        public override void OnCreate()
        {}

        public override void OnDestroy()
        {}

        public override void OnDisconnect(IConnection connection, ulong userId)
        {}

        public override void ProcessPacket(IConnection sender, byte[] data)
        {
            ulong owner = BitConverter.ToUInt64(data, 1 + 8);

            if(owner == Clients[sender].Id)
                NetworkEntity.HandlePacket(_entityManager, data, 0);
        }

        public override void Update(float deltatime)
        {
            foreach (var entity in _entityManager.OfType<NetworkEntity>())
                BroadCaster.EnqueuePacket(entity.GetPacket());
        }
    }
}
