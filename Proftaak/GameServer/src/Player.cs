using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitySystem;
using Networking;

namespace GameServer.Entities
{
    struct PlayerPacket
    {
        public float X, Y, Z;
    }

    class Player : NetworkEntity<PlayerPacket>
    {
        private float _x, _y, _z;

        public Player(ulong id, ulong ownerId) : base(id, ownerId, Type.PLAYER)
        {}

        public override void FixedUpdate(EntityManager entityManager, float deltatime)
        {}

        public override PlayerPacket GetNetworkData()
        {
            return new PlayerPacket() {
               X = _x, Y = _y, Z = _z
            };
        }

        public override void NetworkUpdate(PlayerPacket packet)
        {
            _x = packet.X;
            _y = packet.Y;
            _z = packet.Z;
        }

        public override void OnAdd(EntityManager entityManager)
        {}

        public override void OnRemove(EntityManager entityManager)
        {}

        public override void Update(EntityManager entityManager, float deltatime)
        {}
    }
}
