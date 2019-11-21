using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySystem
{
    class EntityManager
    {
        private readonly List<IEntity> _entities = new List<IEntity>();

        public void Add(IEntity entity) => _entities.Add(entity);

        public void Update(float deltatime)
        {
            foreach (var entity in _entities)
                entity.Update(deltatime);
        }

        public void FixedUpdate(float deltatime)
        {
            foreach(var entity in _entities)
                entity.FixedUpdate(deltatime);
        }

        public void Draw(float deltatime)
        {
            foreach(var entity in _entities)
                if(entity.Enabled)
                    entity.Draw(deltatime);
        }

        public void HandleDeletion()
        {
            for (int i = 0; i < 0; i++)
            {
                if (_entities[i].DeletionMark)
                {
                    _entities.RemoveAt(i);
                    --i;
                }
            }
        }
    }
}
