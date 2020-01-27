using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitySystem;
using Game.Engine.Input;
using Game.Engine.Maths;
using Game.Engine.Rendering;
using Networking;
using OpenTK;

namespace Game.Gameplay
{
    struct PlayerPacket
    {
        public float x, y, z;
    }

    class Player : NetworkEntity<PlayerPacket>
    {
        public VoxelModel _modelBody; //Merely a reference, not enough time to think all this through, so it's public.
        private Transform _transform;
        private bool _controllable;
        private Camera camera;

        private float _speed = 7f;

        public Player(ulong id, ulong ownerId, bool controllable) : base(id, ownerId, Type.PLAYER) {
            _transform = new Transform();
            _transform.Position = new OpenTK.Vector3(0.0f, 32.0f, 0.0f);
            _controllable = controllable;
        }
        public override void FixedUpdate(EntityManager entityManager, float deltatime)
        {
            //throw new NotImplementedException();
            _modelBody.Transform.Position = _transform.Position;
        }

        public override PlayerPacket GetNetworkData()
        {
            return new PlayerPacket() { x=0,y=0,z=0 };
        }

        public override void NetworkUpdate(PlayerPacket packet)
        {
            //throw new NotImplementedException();
            _transform.Position = new OpenTK.Vector3(packet.x, packet.y, packet.z);
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
                bool pressedFD = KeyboardInput.IsForwardDown();
                bool pressedBD = KeyboardInput.IsBackwardDown();
                bool pressedSLD = KeyboardInput.IsStrafeLeftDown();
                bool pressedSRD = KeyboardInput.IsStrafeRightDown();

                if (pressedFD == true)
                {
                    _transform.Position += new Vector3(_speed * (float)Math.Sin(_transform.Rotation.Y) * deltatime, 0f, _speed * (float)Math.Cos(_transform.Rotation.Y) * deltatime);
                }
                if (pressedBD == true)
                {
                    //_transform.Position += new Vector3(1 * deltatime, 0f, 0f);
                    _transform.Position -= new Vector3(_speed * (float)Math.Sin(_transform.Rotation.Y) * deltatime, 0f, _speed * (float)Math.Cos(_transform.Rotation.Y) * deltatime);
                }
                if (pressedSLD == true)
                {
                    //_transform.Position += new Vector3(1 * deltatime, 0f, 0f);
                    _transform.Position -= new Vector3(_speed * (float)Math.Cos(-_transform.Rotation.Y) * deltatime, 0f, _speed * (float)Math.Sin(-_transform.Rotation.Y) * deltatime);
                }
                if (pressedSRD == true)
                {
                    //_transform.Position += new Vector3(1 * deltatime, 0f, 0f);
                    _transform.Position += new Vector3(_speed * (float)Math.Cos(-_transform.Rotation.Y) * deltatime, 0f, _speed * (float)Math.Sin(-_transform.Rotation.Y) * deltatime);
                }
            }
        }
    }
}
