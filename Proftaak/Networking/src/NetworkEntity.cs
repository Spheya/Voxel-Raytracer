using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EntitySystem;

namespace Networking
{
    public abstract class NetworkEntity : IEntity
    {
        public bool Read { get; set; }
        public long Id { get; }

        public bool DeletionMark { get; set; }
        public bool Enabled { get; set; }

        protected NetworkEntity(bool read, long id)
        {
            Read = read;
            Id = id;
        }

        public abstract void OnAdd(EntityManager entityManager);

        public abstract void Update(EntityManager entityManager, float deltatime);

        public abstract void FixedUpdate(EntityManager entityManager, float deltatime);

        public abstract void OnRemove(EntityManager entityManager);

        public void NetworkUpdate(NetworkEntityBuffer networkEntityBuffer)
        {
            int index = 0;
            byte[] data = networkEntityBuffer[Id].ToArray();
            foreach (FieldInfo field in GetType().GetFields())
            {
                foreach (Attribute attribute in field.GetCustomAttributes())
                {
                    if (attribute is NetworkVariable)
                    {
                        if (Read)
                        {
                            object val = Activator.CreateInstance(field.GetType());

                            int size = Marshal.SizeOf(field.GetValue(this));
                            IntPtr ptr = Marshal.AllocHGlobal(size);

                            Marshal.Copy(data, 0, ptr, size);

                            val = Convert.ChangeType(Marshal.PtrToStructure(ptr, field.GetType()), field.FieldType);
                            Marshal.FreeHGlobal(ptr);

                            field.SetValue(this, val);

                            data = data.Skip(size).ToArray();
                        }
                        else
                        {
                            object value = field.GetValue(this);
                            int size = Marshal.SizeOf(value);
                            byte[] arr = new byte[size];

                            IntPtr ptr = Marshal.AllocHGlobal(size);
                            Marshal.StructureToPtr(value, ptr, true);
                            Marshal.Copy(ptr, arr, 0, size);
                            Marshal.FreeHGlobal(ptr);

                            for (int i = 0; i < size; i++)
                                networkEntityBuffer[Id][index + i] = arr[i];

                            index += size;
                        }

                        break;
                    }
                }
            }
        }
    }
}
