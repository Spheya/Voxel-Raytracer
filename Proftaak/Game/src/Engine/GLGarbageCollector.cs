using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine
{
    /// <summary>
    /// A class that allows OpenGL objects to be destroyed on the thread that owns the context
    /// (.NET garbage collector runs on a different thread)
    /// </summary>
    class GLGarbageCollector
    {
        private static readonly Queue<int> _shaders = new Queue<int>();
        private static readonly Queue<int> _programs = new Queue<int>();
        private static readonly Queue<int> _buffers = new Queue<int>();
        private static readonly Queue<int> _vertexArrays = new Queue<int>();
        private static readonly Queue<int> _textures = new Queue<int>();

        /// <summary>
        /// Add a shader to the deletion queue
        /// </summary>
        /// <param name="shader">The shader that needs to be deleted</param>
        public static void AddShader(int shader)
        {
            lock (_shaders)
                _shaders.Enqueue(shader);
        }

        /// <summary>
        /// Add a program to the deletion queue
        /// </summary>
        /// <param name="program">The program that needs to be deleted</param>
        public static void AddProgram(int program)
        {
            lock (_programs)
                _programs.Enqueue(program);
        }

        /// <summary>
        /// Add a buffer to the deletion queue
        /// </summary>
        /// <param name="buffer">The buffer that needs to be deleted</param>
        public static void AddBuffer(int buffer)
        {
            lock (_buffers)
                _buffers.Enqueue(buffer);
        }

        /// <summary>
        /// Add a vertexarray to the deletion queue
        /// </summary>
        /// <param name="vao">The vertexarray that needs to be deleted</param>
        public static void AddVertexArray(int vao)
        {
            lock (_vertexArrays)
                _vertexArrays.Enqueue(vao);
        }

        /// <summary>
        /// Add a texture to the deletion queue
        /// </summary>
        /// <param name="texture">The texture that needs to be deleted</param>
        public static void AddTexture(int texture)
        {
            lock (_textures)
                _textures.Enqueue(texture);
        }

        /// <summary>
        /// Make OpenGL calls on every item that should be deleted
        /// </summary>
        public static void Process()
        {
            lock(_shaders)
                while(_shaders.Count > 0)
                    GL.DeleteShader(_shaders.Dequeue());

            lock(_programs)
                while(_programs.Count > 0)
                    GL.DeleteProgram(_programs.Dequeue());

            lock(_buffers)
                while(_buffers.Count > 0)
                    GL.DeleteBuffer(_buffers.Dequeue());

            lock(_vertexArrays)
                while(_vertexArrays.Count > 0)
                    GL.DeleteVertexArray(_vertexArrays.Dequeue());

            lock(_textures)
                while(_textures.Count > 0)
                    GL.DeleteTexture(_textures.Dequeue());
        }
    }
}
