using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine.Rendering
{
    class BufferTexture<T> : IEnumerable<T>
        where T : struct
    {
        private readonly List<T> _bufferData = new List<T>();

        private int _bufferSize = 0;

        private readonly int _bufferTextureId;
        private readonly int _bufferId;

        private readonly SizedInternalFormat _internalFormat;

        private bool _dirtyFlag;
        private int _minDirty = int.MaxValue;
        private int _maxDirty = int.MinValue;

        /// <summary>
        /// Get or set a value in the buffertexture
        /// </summary>
        /// <param name="index">The index of the value</param>
        /// <returns>The value at the given index in the buffertexture</returns>
        public T this[int index]
        {
            get => _bufferData[index];
            set
            {
                _bufferData[index] = value;

                _minDirty = Math.Min(_minDirty, index);
                _maxDirty = Math.Max(_maxDirty, index + 1);
                _dirtyFlag = true;
            }
        }

        /// <summary>
        /// The size of the buffer
        /// </summary>
        public int Count => _bufferData.Count;

        /// <summary>
        /// Removes some data from the buffer
        /// </summary>
        /// <param name="index">The index of the first element you want to remove</param>
        /// <param name="size">The amount of elements you want to remove</param>
        public void Erase(int index, int size = 1) => _bufferData.RemoveRange(index, size);

        /// <summary>
        /// Add data to the buffer
        /// </summary>
        /// <param name="t"></param>
        public void Add(T t) => _bufferData.Add(t);

        /// <summary>
        /// Append a collection to the buffer
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IEnumerable<T> collection) => _bufferData.AddRange(collection);

        /// <param name="format">The internal format of the buffer texture</param>
        public BufferTexture(SizedInternalFormat format)
        {
            _internalFormat = format;

            // Generate the buffer
            _bufferId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.TextureBuffer, _bufferId);
            GL.BufferData(BufferTarget.TextureBuffer, _bufferData.Count * Marshal.SizeOf(default(T)), _bufferData.ToArray(), BufferUsageHint.DynamicDraw);
            _bufferSize = _bufferData.Count;
            _bufferTextureId = GL.GenTexture();
            GL.BindBuffer(BufferTarget.TextureBuffer, 0);

            GL.BindTexture(TextureTarget.TextureBuffer, _bufferTextureId);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, _internalFormat, _bufferId);
            GL.BindTexture(TextureTarget.TextureBuffer, 0);
        }

        /// <summary>
        /// Update the texture on the gpu
        /// </summary>
        public void Update()
        {
            if (_dirtyFlag)
            {
                GL.BindBuffer(BufferTarget.TextureBuffer, _bufferId);
                if (_bufferSize >= _bufferData.Count)
                {
                    GL.BufferSubData(BufferTarget.TextureBuffer, IntPtr.Zero + _minDirty, (_maxDirty - _minDirty) * Marshal.SizeOf(default(T)), _bufferData.Skip(_minDirty).ToArray());
                }
                else
                {
                    GL.BufferData(BufferTarget.TextureBuffer, _bufferData.Count * Marshal.SizeOf(default(T)), _bufferData.ToArray(), BufferUsageHint.DynamicDraw);
                    _bufferSize = _bufferData.Count;
                }
                GL.BindBuffer(BufferTarget.TextureBuffer, 0);

                _maxDirty = int.MinValue;
                _minDirty = int.MaxValue;

                _dirtyFlag = false;
            }
        }

        /// <summary>
        /// Bind the texture to a texture unit
        /// </summary>
        /// <param name="unit">The unit you want to bind the texture to</param>
        public void Bind(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.TextureBuffer, _bufferTextureId);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _bufferData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _bufferData.GetEnumerator();
        }

        ~BufferTexture()
        {
            GLGarbageCollector.AddBuffer(_bufferId);
            GLGarbageCollector.AddTexture(_bufferTextureId);
        }
    }
}
