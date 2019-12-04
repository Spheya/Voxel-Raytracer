using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelData;

namespace Game.Engine.Rendering
{
    class DistanceField
    {
        private readonly VoxelGrid _data;
        private readonly BufferTexture<byte> _storage;
        private readonly int _storageOffset;

        private byte this[int x, int y, int z]
        {
            get => _storage[x + y * _data.Width + z * _data.Width * _data.Height + _storageOffset];
            set => _storage[x + y * _data.Width + z * _data.Width * _data.Height + _storageOffset] = value;
        }

        public DistanceField(VoxelGrid data, BufferTexture<byte> storage, int storageOffset)
        {
            _data = data;
            _storage = storage;
            _storageOffset = storageOffset;

 //           Build();
        }

        public void OnVoxelAdd(int x, int y, int z)
        {
 //           Build();
        }

        public void OnVoxelRemove(int x, int y, int z)
        {
   //         Build();
        }

        private bool FindInRing(int x, int y, int z, int range)
        {
            for(int xo = -range; xo <= range; xo++)
            {
                for (int yo = -range; yo <= range; yo++)
                {
                    for (int zo = -range; zo <= range; zo++)
                    {
                        if (!(xo > -range && xo < range && yo > -range && yo < range && zo > -range && zo < range))
                        {
                            if (!(x + xo < 0 || x + xo >= _data.Width || y + yo < 0 || y + yo >= _data.Height ||
                                z + zo < 0 || z + zo >= _data.Depth))
                            {
                                if (_data[x + xo, y + yo, z + zo] != 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void Build()
        {
            const int limit = 8;

            for (int x = 0; x < _data.Width; x++)
            {
                Console.WriteLine(x / (float)_data.Width * 100.0f);
                for (int y = 0; y < _data.Height; y++)
                {
                    for (int z = 0; z < _data.Depth; z++)
                    {
                        bool foundValue = false;
                        for (int range = 0; range < limit; range++)
                        {
                            if (FindInRing(x,y,z,range))
                            {
                                foundValue = true;
                                this[x, y, z] = (byte)Math.Min(range, byte.MaxValue);
                                break;
                            }
                        }

                        if (!foundValue)
                            this[x, y, z] = limit;
;                    }
                }
            }
        }
    }
}
