using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelData
{
    public class VoxelGrid
    {
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }

        private readonly byte[,,] _voxels;

        public ushort[] VoxelMaterials { get; }

        public byte this[int x, int y, int z]
        {
            get => _voxels[x, y, z];
            set {
                _voxels[x, y, z] = value;

                VoxelMaterials[x + y * Width + z * Width * Height] = value;
            }
        }

        public VoxelGrid(int width, int height, int depth)
        {
            Debug.Assert(width > 0);
            Debug.Assert(height > 0);
            Debug.Assert(depth > 0);

            Width = width;
            Height = height;
            Depth = depth;

            _voxels = new byte[width, height, depth];

            VoxelMaterials = new ushort[width * height * depth];
        }

        public VoxelGrid(int width, int height, int depth, byte[,,] data)
        {
            Debug.Assert(width > 0);
            Debug.Assert(height > 0);
            Debug.Assert(depth > 0);

            Width = width;
            Height = height;
            Depth = depth;

            _voxels = data;

            VoxelMaterials = new ushort[width * height * depth];
        }
    }
}
