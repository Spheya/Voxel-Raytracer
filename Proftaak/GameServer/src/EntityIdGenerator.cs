using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    class EntityIdGenerator
    {
        private ulong _id = 0;
        public ulong Generate()
        {
            return _id++;
        }
    }
}
