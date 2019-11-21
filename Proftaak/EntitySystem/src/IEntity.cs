using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySystem
{
    interface IEntity
    {
        bool DeletionMark { get; set; }
        bool Enabled { get; set; }

        void Update(float deltatime);
        void FixedUpdate(float deltatime);
        void Draw(float deltatime);
    }
}
