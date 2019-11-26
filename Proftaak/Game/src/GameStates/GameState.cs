using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Rendering;
using Game.Engine.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using VoxelData;

namespace Game.GameStates
{
    sealed class GameState : ApplicationState
    {
        int VertexArray;
        int Buffer;

        private VoxelModel model;

        Vector2[] QuadVertices = new Vector2[4] {
            new Vector2(-1f, -1f),
            new Vector2(1f, -1f),
            new Vector2(1f, 1f),
            new Vector2(-1f, 1f)
        };

        private ShaderProgram _shader;

        public override void OnCreate()
        {

            try
            {
                Shader vertexShader = new Shader(ShaderType.VertexShader, 
                    ShaderPreprocessor.Execute("", File.ReadAllLines(@"res\vertex.glsl"), @"res\"));
                Shader fragmentShader = new Shader(ShaderType.FragmentShader, 
                    ShaderPreprocessor.Execute("", File.ReadAllLines(@"res\fragment.glsl"), @"res\"));

                _shader = new ShaderProgram(new[] { vertexShader, fragmentShader });
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            Console.WriteLine("Shader compiled <o/"); //epic it work

            //Epic
            VertexArray = GL.GenVertexArray();
            Buffer = GL.GenBuffer();

            GL.BindVertexArray(VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);

            GL.NamedBufferStorage(
                Buffer,
                8 * 4,
                QuadVertices,
                BufferStorageFlags.MapWriteBit
            );

            GL.VertexArrayAttribBinding(VertexArray, 0, 0);
            GL.EnableVertexArrayAttrib(VertexArray, 0);
            GL.VertexArrayAttribFormat(
                VertexArray,
                0,                      // attribute index, from the shader location = 0
                2,                      // size of attribute, vec2
                VertexAttribType.Float, // contains floats
                false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
                0);                     // relative offset, first item

            GL.VertexArrayVertexBuffer(VertexArray, 0, Buffer, IntPtr.Zero, 8);

            model = new VoxelModel(32, 32, 32);
            for (int x = 0; x < 32; x++)
            for (int y = 0; y < 32; y++)
            for (int z = 0; z < 32; z++)
                model[x, y, z] = new Voxel(1);

            Console.WriteLine("Epic");
        }

        public override void OnUpdate(float deltatime)
        {
        }

        public override void OnFixedUpdate(float deltatime)
        {
        }

        private float f;
        public override void OnDraw(float deltatime)
        {

            f += deltatime;

            model.UpdateBufferTexture();

            _shader.Bind();

            GL.BindVertexArray(VertexArray);

            model.BindTexture(TextureUnit.Texture0);
            GL.Uniform1(_shader.GetUniformLocation("u_voxelBuffer"), 1, new[] { 0 });
            GL.Uniform3(_shader.GetUniformLocation("u_bufferDimensions"), 1, new[] { model.Width, model.Height, model.Depth });
            GL.Uniform2(_shader.GetUniformLocation("u_windowSize"), 1, new float [] { Window.Width, Window.Height });
            GL.Uniform1(_shader.GetUniformLocation("u_zoom"), 1, new []{ (Window.Height * 0.5f) / (float)Math.Tan(90.0f * (Math.PI / 360.0f)) });
            GL.Uniform1(_shader.GetUniformLocation("f"), 1, new []{ f });

            GL.DrawArrays(PrimitiveType.TriangleFan, 0,4);

            _shader.Unbind();
        }

        public override void OnDestroy()
        {
        }
    }
}
