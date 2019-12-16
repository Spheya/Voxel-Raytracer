using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Maths;
using VoxelData;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine.Rendering
{
    class VoxelModel
    {
        private readonly VoxelGrid _grid;

        private readonly BufferTexture<byte> _target;
        
        /// <summary>
        /// How the model is positioned in the world
        /// </summary>
        public Transform Transform { get; }

        /// <summary>
        /// The width of the underlying grid
        /// </summary>
        public int Width => _grid.Width;
        
        /// <summary>
        /// The height of the underlying grid
        /// </summary>
        public int Height => _grid.Height;

        /// <summary>
        /// The depth of the underlying grid
        /// </summary>
        public int Depth => _grid.Depth;

        /// <summary>
        /// The amount of ushorts this object stores into the buffertexture
        /// </summary>
        public int Footprint => Width * Height * Depth;

        /// <summary>
        /// The offset into the buffertexture
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Write a voxel into the underlying grid and the buffertexture
        /// </summary>
        /// <param name="x">The x coordinate of the voxel</param>
        /// <param name="y">The y coordinate of the voxel</param>
        /// <param name="z">The z coordinate of the voxel</param>
        /// <returns></returns>
        public byte this[int x, int y, int z]
        {
            get => _grid[x, y, z];
            set
            {
                _grid[x, y, z] = value;

                // Mark the bounds of all the changes since the previous update
                int index = Offset + x + y * Width + z * Width * Height;
                _target[index] = value;
            }
        }

        /// <param name="target">The buffertexture this should be part of</param>
        /// <param name="offset">The offset into the buffertexture</param>
        /// <param name="width">The width of the grid</param>
        /// <param name="height">The height of the grid</param>
        /// <param name="depth">The depth of the grid</param>
        /// <param name="transform">How the model should be positioned in the world</param>
        public VoxelModel(BufferTexture<byte> target, int offset, int width, int height, int depth, Transform transform) :
            this(target, offset, width, height, depth)
        {
            Transform = transform;
        }

        /// <param name="target">The buffertexture this should be part of</param>
        /// <param name="offset">The offset into the buffertexture</param>
        /// <param name="width">The width of the grid</param>
        /// <param name="height">The height of the grid</param>
        /// <param name="depth">The depth of the grid</param>
        public VoxelModel(BufferTexture<byte> target, int offset, int width, int height, int depth)
        {
            _target = target;
            _grid = new VoxelGrid(width, height, depth);
            Offset = offset;
            Transform = new Transform(Vector3.Zero, Vector3.Zero, new Vector3(width, height, depth));
        }
    }
}
