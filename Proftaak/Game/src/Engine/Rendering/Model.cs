using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine.Rendering
{
    class Model
    {
        private readonly int _vbo;

        public int Vao { get; }

        public PrimitiveType Type { get; }
        public int Count { get; }

        public Model(float[] vertexData, int dimensions, PrimitiveType type)
        {
            Count = vertexData.Length / dimensions;
            Type = type;
            Vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();

            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

            GL.NamedBufferStorage(
                _vbo,
                vertexData.Length * sizeof(float),
                vertexData,
                BufferStorageFlags.MapWriteBit
            );

            GL.VertexArrayAttribBinding(Vao, 0, 0);
            GL.EnableVertexArrayAttrib(Vao, 0);
            GL.VertexArrayAttribFormat(
                Vao,
                0,                          // attribute index, from the shader location = 0
                dimensions,                 // size of attribute, vec2
                VertexAttribType.Float,     // contains floats
                false,                      // does not need to be normalized as it is already, floats ignore this flag anyway
                0);                         // relative offset, first item

            GL.VertexArrayVertexBuffer(Vao, 0, _vbo, IntPtr.Zero, dimensions * sizeof(float));
        }

        ~Model()
        {
            //TODO: Fix these runtime errors
            GL.DeleteBuffer(_vbo);
            GL.DeleteVertexArray(Vao);
        }
    }
}
