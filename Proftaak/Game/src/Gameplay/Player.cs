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
        private Vector3 _velocity;
        private bool _controllable;
        public Camera camera;

        public VoxelModel _world;

        private float _speed = 7f;
        private float mouseSensitivity = 0.001f;

        public Player(ulong id, ulong ownerId, bool controllable) : base(id, ownerId, Type.PLAYER) {
            _transform = new Transform();
            _transform.Position = new OpenTK.Vector3(0.0f, 32.0f, 0.0f);
            _transform.Rotation = new OpenTK.Vector3(0f, 0f, 0f);
            _controllable = controllable;
            camera = new Camera(_transform.Position, _transform.Rotation);
        }
        public override void FixedUpdate(EntityManager entityManager, float deltatime)
        {
            //throw new NotImplementedException();
            _modelBody.Transform.Position = _transform.Position;

            if (_controllable)
            {
                // Check physics
                //if (_transform.Position.X >= _world.Transform.Position.X - _world.Width && _transform.Position.X < _world.Transform.Position.X + _world.Width)
                //    if (_transform.Position.Y >= _world.Transform.Position.Y - _world.Height && _transform.Position.Y < _world.Transform.Position.Y + _world.Height)
                //        if (_transform.Position.Z >= _world.Transform.Position.Z - _world.Depth && _transform.Position.Z < _world.Transform.Position.Z + _world.Depth)
                //        {
                //            byte voxel = _world[(int)_transform.Position.X, (int)_transform.Position.Y - 1, (int)_transform.Position.Z];
                //            if (voxel > 0)
                //            {
                //                //Player is on top of voxel
                //                _velocity.Y = 0f;
                //            }
                //            else
                //            {
                //                _velocity.Y += 9.8f * deltatime;
                //            }
                //        }

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

                Vector2 delta = MouseInput.GetMouseDelta() * mouseSensitivity;
                _transform.Rotation += new Vector3(delta.Y, delta.X, 0.0f);

                camera._transform.Position = _transform.Position + new Vector3(0f, 4f, 0f);
                camera._transform.Rotation = _transform.Rotation;

                //Console.WriteLine($"X: {camera._transform.Position.X}");
            }
        }

        public override PlayerPacket GetNetworkData()
        {
            return new PlayerPacket() { x=_transform.Position.X,y=_transform.Position.Y,z=_transform.Position.Z };
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
            Console.WriteLine("test");
            //throw new NotImplementedException();
        }
    }
}
