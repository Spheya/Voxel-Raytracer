using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using VoxelData;

namespace Game.GameStates
{
    sealed class GameState : ApplicationState
    {
        int Shader;

        int VertexArray;
        int Buffer;

        private VoxelModel model;

        Vector2[] QuadVertices = new Vector2[4] {
            new Vector2(-1f, -1f),
            new Vector2(1f, -1f),
            new Vector2(1f, 1f),
            new Vector2(-1f, 1f)
        };

        public override void OnCreate()
        {
            Random rand = new Random();
            model = new VoxelModel(32, 32, 32);
            for(int x = 0; x < 32; x++)
                for(int y = 0; y < 32; y++)
                    for(int z = 0; z < 32; z++) 
                        model[x,y,z] = new Voxel(1);

            try
            {
                var vertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexShader, File.ReadAllText(@"res\vertex.glsl"));
                GL.CompileShader(vertexShader);
                var info = GL.GetShaderInfoLog(vertexShader);
                if (!string.IsNullOrWhiteSpace(info))
                    throw new Exception($"CompileShader {ShaderType.VertexShader} had errors: {info}");

                var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentShader, File.ReadAllText(@"res\fragment.glsl"));
                GL.CompileShader(fragmentShader);
                info = GL.GetShaderInfoLog(fragmentShader);
                if (!string.IsNullOrWhiteSpace(info))
                    throw new Exception($"CompileShader {ShaderType.FragmentShader} had errors: {info}");

                Shader = GL.CreateProgram();
                GL.AttachShader(Shader, vertexShader);
                GL.AttachShader(Shader, fragmentShader);
                GL.LinkProgram(Shader);

                info = GL.GetProgramInfoLog(Shader);
                if (!string.IsNullOrWhiteSpace(info))
                    throw new Exception($"CompileShaders ProgramLinking had errors: {info}");

                GL.DetachShader(Shader, vertexShader);
                GL.DetachShader(Shader, fragmentShader);
                GL.DeleteShader(vertexShader);
                GL.DeleteShader(fragmentShader);
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

            GL.UseProgram(Shader);
            GL.BindVertexArray(VertexArray);

            model.BindTexture(TextureUnit.Texture0);
            GL.Uniform1(GL.GetUniformLocation(Shader, "u_voxelBuffer"), 1, new[] { 0 });
            GL.Uniform3(GL.GetUniformLocation(Shader, "u_bufferDimensions"), 1, new[] { model.Width, model.Height, model.Depth });
            GL.Uniform2(GL.GetUniformLocation(Shader, "u_windowSize"), 1, new float [] { Window.Width, Window.Height });
            GL.Uniform1(GL.GetUniformLocation(Shader, "u_zoom"), 1, new []{ (Window.Height * 0.5f) / (float)Math.Tan(90.0f * (Math.PI / 360.0f)) });
            GL.Uniform1(GL.GetUniformLocation(Shader, "f"), 1, new []{ f });

            GL.DrawArrays(PrimitiveType.TriangleFan, 0,4);
        }

        public override void OnDestroy()
        {
        }
    }
}
