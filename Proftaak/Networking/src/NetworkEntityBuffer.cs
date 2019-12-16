using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public class NetworkEntityBuffer
    {
        private readonly Dictionary<long, List<byte>> _packets = new Dictionary<long, List<byte>>();
        private readonly List<long> _availableIds = new List<long>();

        public List<byte> this[long id]
        {
            get
            {
                if (!_packets.ContainsKey(id))
                    _packets.Add(id, new List<byte>());
                return _packets[id];
            }
        }

        public long GenerateId()
        {
            if (_availableIds.Count == 0)
                return _packets.Count;
            return _availableIds[0];
        }

        public bool Remove(long id)
        {
            if (_packets.Remove(id))
            {
                _availableIds.Add(id);
                return true;
            }

            return false;
        }
    }
}
