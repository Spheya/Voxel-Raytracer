using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitySystem;

namespace Networking
{
    public abstract class NetworkEntity : IEntity
    {
        public bool Read { get; set; }

        public bool DeletionMark { get; set; }
        public bool Enabled { get; set; }

        protected NetworkEntity(bool read)
        {
            Read = read;
        }

        public abstract void OnAdd(EntityManager entityManager);

        public abstract void Update(EntityManager entityManager, float deltatime);

        public abstract void FixedUpdate(EntityManager entityManager, float deltatime);

        public abstract void OnRemove(EntityManager entityManager);
    }
}
