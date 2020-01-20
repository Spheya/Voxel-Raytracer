using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelData;

namespace EntitySystem
{
    public interface IEntity
    {
        bool DeletionMark { get; set; }
        bool Enabled { get; set; }

        void OnAdd(EntityManager entityManager);
        void Update(EntityManager entityManager, float deltatime);
        void FixedUpdate(EntityManager entityManager, float deltatime);
        void OnRemove(EntityManager entityManager);
    }
}
