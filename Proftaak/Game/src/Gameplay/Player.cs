using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitySystem;
using Networking;

namespace Game.Gameplay
{
    struct PlayerPacket
    {
        public float x, y, z;
    }

    class Player : NetworkEntity<PlayerPacket>
    {
        public Player(ulong id, ulong ownerId) : base(id, ownerId, Type.PLAYER) { }
        public override void FixedUpdate(EntityManager entityManager, float deltatime)
        {
            throw new NotImplementedException();
        }

        public override PlayerPacket GetNetworkData()
        {
            return new PlayerPacket() { x=0,y=0,z=0 };
        }

        public override void NetworkUpdate(PlayerPacket packet)
        {
            throw new NotImplementedException();
        }

        public override void OnAdd(EntityManager entityManager)
        {
            //throw new NotImplementedException();
        }

        public override void OnRemove(EntityManager entityManager)
        {
            //throw new NotImplementedException();
        }

        public override void Update(EntityManager entityManager, float deltatime)
        {
            //throw new NotImplementedException();
        }
    }
}
