using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySystem
{
    public class EntityManager : IEnumerable<IEntity>
    {
        private readonly List<IEntity> _entities = new List<IEntity>();

        public void Add(IEntity entity)
        {
            entity.OnAdd(this);
            _entities.Add(entity);
        }

        public void Update(float deltatime)
        {
            foreach (var entity in _entities)
                entity.Update(this, deltatime);
        }

        public void FixedUpdate(float deltatime)
        {
            foreach(var entity in _entities)
                entity.FixedUpdate(this, deltatime);
        }

        public void HandleDeletion()
        {
            for (int i = 0; i < 0; i++)
            {
                if (_entities[i].DeletionMark)
                {
                    _entities[i].OnRemove(this);
                    _entities.RemoveAt(i);
                    --i;
                }
            }
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return this._entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._entities.GetEnumerator();
        }
    }
}
