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
        public Transform Transform { get; }
        private readonly VoxelGrid _grid;

        private bool _dirtyFlag;
        private int _minDirty = int.MaxValue;
        private int _maxDirty = int.MinValue;

        private readonly int _bufferTextureId;
        private readonly int _bufferId;

        public int Width => _grid.Width;
        public int Height => _grid.Height;
        public int Depth => _grid.Depth;

        public Voxel this[int x, int y, int z]
        {
            get => _grid[x, y, z];
            set
            {
                _grid[x, y, z] = value;

                // Mark the bounds of all the changes since the previous update
                int index = x + y * Width + z * Width * Height;
                _minDirty = Math.Min(_minDirty, index);
                _maxDirty = Math.Max(_maxDirty, index + 1);

                _dirtyFlag = true;
            }
        }

        public VoxelModel(int width, int height, int depth, Transform transform) :
            this(width, height, depth)
        {
            Transform = transform;
        }

        public VoxelModel(int width, int height, int depth)
        {
            Transform = new Transform(Vector3.Zero, Vector3.Zero, new Vector3(width, height, depth));

            _grid = new VoxelGrid(width, height, depth);

            // Generate the buffer
            _bufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.TextureBuffer, _bufferId);
            GL.BufferData(BufferTarget.TextureBuffer, sizeof(ushort) * _grid.VoxelMaterials.Length, _grid.VoxelMaterials, BufferUsageHint.DynamicDraw);
            _bufferTextureId = GL.GenTexture();
            GL.BindBuffer(BufferTarget.TextureBuffer, 0);
        }

        public void UpdateBufferTexture()
        {
            if (_dirtyFlag)
            {
                GL.BindBuffer(BufferTarget.TextureBuffer, _bufferId);
                GL.BufferSubData(BufferTarget.TextureBuffer, IntPtr.Zero + _minDirty, sizeof(ushort) * (_maxDirty - _minDirty), _grid.VoxelMaterials.Skip(_minDirty).ToArray());
                GL.BindBuffer(BufferTarget.TextureBuffer, 0);

                _maxDirty = int.MinValue;
                _minDirty = int.MaxValue;

                _dirtyFlag = false;
            }
        }

        public void BindTexture(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.TextureBuffer, _bufferTextureId);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.R16ui, _bufferId);
        }
    }
}
