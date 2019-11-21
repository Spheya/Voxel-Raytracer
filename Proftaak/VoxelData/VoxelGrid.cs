using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelData
{
    class VoxelGrid
    {
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }

        private readonly Voxel[,,] _voxels;

        public int[] VoxelColours { get; }
        //TODO: Reflectivity 'n shit

        public Voxel this[int x, int y, int z]
        {
            get => _voxels[x, y, z];
            set {
                _voxels[x, y, z] = value;

                VoxelColours[x + y * Width + z * Width * Depth] = value.ColourValue;
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

            _voxels = new Voxel[width, height, depth];

            VoxelColours = new int[width * height * depth];
        }
    }
}
