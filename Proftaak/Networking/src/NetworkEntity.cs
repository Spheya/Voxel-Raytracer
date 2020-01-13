using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using EntitySystem;

namespace Networking
{
    public abstract class _NetworkEntity : IEntity
    {
        public ulong Id { get; }
        public ulong OwnerId { get; }
        public ulong TypeId { get; }

        public bool DeletionMark { get; set; }
        public bool Enabled { get; set; }

        protected _NetworkEntity(ulong id) {
            Id = id;
        }

        public abstract void FixedUpdate(EntityManager entityManager, float deltatime);
        public abstract void OnAdd(EntityManager entityManager);
        public abstract void OnRemove(EntityManager entityManager);
        public abstract void Update(EntityManager entityManager, float deltatime);

        public abstract byte[] GetPacket();
        public abstract void ProcessPacket(byte[] packet, ulong MyId);
    }

    public abstract class NetworkEntity<T> : _NetworkEntity
        where T : struct
    {
        private long _prevTimestamp = long.MinValue;

        protected NetworkEntity(ulong id) : base(id) {}

        public abstract void NetworkUpdate(T packet);
        public abstract T GetNetworkData();

        public override byte[] GetPacket()
        {
            long timestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).Ticks;

            return new byte[] { 0 }.Concat(BitConverter.GetBytes(Id)).Concat(BitConverter.GetBytes(OwnerId)).Concat(BitConverter.GetBytes(timestamp)).Concat(GetBytes(GetNetworkData())).ToArray();
        }

        public override void ProcessPacket(byte[] packet, ulong myId)
        {
            ulong owner = BitConverter.ToUInt64(packet, 1 + 8);
            if (owner != myId)
            {
                long timestamp = BitConverter.ToInt64(packet, 1 + 8 + 8);
                if (timestamp <= _prevTimestamp)
                    return;

                _prevTimestamp = timestamp;

                NetworkUpdate(FromBytes(packet.Skip(1 + 8 + 8 + 8).ToArray()));
            }
        }

        private byte[] GetBytes(T str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        private T FromBytes(byte[] arr)
        {
            T str = new T();

            int size = Marshal.SizeOf(str);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);

            str = (T)Marshal.PtrToStructure(ptr, str.GetType());
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        public static void HandlePacket(EntityManager manager, byte[] data, ulong myId)
        {
            if (data[0] == 0)
            {
                ulong id = BitConverter.ToUInt64(data, 1);

                foreach (var entity in manager.OfType<_NetworkEntity>())
                {
                    if(entity.Id == id)
                    {
                        entity.ProcessPacket(myId);
                    }
                }
            }
        }
    }
}
