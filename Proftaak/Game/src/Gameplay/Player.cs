using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitySystem;
using Game.Engine.Maths;
using Game.Engine.Rendering;
using Networking;

namespace Game.Gameplay
{
    struct PlayerPacket
    {
        public float x, y, z;
    }

    class Player : NetworkEntity<PlayerPacket>
    {
        public VoxelModel _modelBody; //Merely a reference, not enough time to think all this through, so it's public.
        private Transform transform;
        private bool _controllable;

        public Player(ulong id, ulong ownerId, bool controllable) : base(id, ownerId, Type.PLAYER) {
            transform = new Transform();
            transform.Position = new OpenTK.Vector3(0.0f, 32.0f, 0.0f);
            _controllable = controllable;
        }
        public override void FixedUpdate(EntityManager entityManager, float deltatime)
        {
            //throw new NotImplementedException();
            _modelBody.Transform.Position = transform.Position;
        }

        public override PlayerPacket GetNetworkData()
        {
            return new PlayerPacket() { x=0,y=0,z=0 };
        }

        public override void NetworkUpdate(PlayerPacket packet)
        {
            //throw new NotImplementedException();
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
            if (_controllable)
            {
                //Take input and move player
            }
        }
    }
}
