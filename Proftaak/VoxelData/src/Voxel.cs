using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelData
{
    public struct Voxel
    {
        public int ColourValue => (_r << 16) | (_g << 8) | (_b << 0);

        public byte _r, _g, _b;

        public Voxel(int rgb)
        {
            _r = (byte)((rgb >> 16) & 0xFF);
            _g = (byte)((rgb >>  8) & 0xFF);
            _b = (byte)((rgb >>  0) & 0xFF);
        }

        public Voxel(byte r = 0xFF, byte g = 0xFF, byte b = 0xFF)
        {
            _r = r;
            _g = g;
            _b = b;
        }
    }
}
