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
        private Model _canvas;

        private VoxelModel _model;

        private ShaderProgram _shader;

        public override void OnCreate()
        {
            try
            {
                Shader vertexShader = new Shader(ShaderType.VertexShader, 
                    ShaderPreprocessor.Execute(null, File.ReadAllLines(@"res\vertex.glsl"), @"res\"));
                Shader fragmentShader = new Shader(ShaderType.FragmentShader, 
                    ShaderPreprocessor.Execute(null, File.ReadAllLines(@"res\fragment.glsl"), @"res\"));

                _shader = new ShaderProgram(new[] { vertexShader, fragmentShader });
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            Console.WriteLine("Shader compiled <o/"); //epic it work

            _canvas = new Model(new []{
                -1.0f, -1.0f,
                 1.0f, -1.0f,
                 1.0f,  1.0f,
                -1.0f,  1.0f
            }, 2, PrimitiveType.TriangleFan);

            _model = new VoxelModel(32, 32, 32);
            for (int x = 0; x < 32; x++)
            for (int y = 0; y < 32; y++)
            for (int z = 0; z < 32; z++)
                _model[x,y,z] = new Voxel(1);

            Console.WriteLine("Epic");
        }

        public override void OnUpdate(float deltatime)
        {
            Random rand = new Random();
            _model[rand.Next(_model.Width), rand.Next(_model.Height), rand.Next(_model.Depth)] = new Voxel(1);
        }

        public override void OnFixedUpdate(float deltatime)
        {
        }

        private float f;
        public override void OnDraw(float deltatime)
        {
            Random rand = new Random();

            _model[rand.Next(_model.Width), rand.Next(_model.Height), rand.Next(_model.Depth)] = new Voxel(1);

            f += deltatime;

            _model.UpdateBufferTexture();

            _shader.Bind();

            GL.BindVertexArray(_canvas.Vao);

            _model.BindTexture(TextureUnit.Texture0);
            GL.Uniform1(_shader.GetUniformLocation("u_voxelBuffer"), 1, new[] { 0 });
            GL.Uniform3(_shader.GetUniformLocation("u_bufferDimensions"), 1, new[] { _model.Width, _model.Height, _model.Depth });
            GL.Uniform2(_shader.GetUniformLocation("u_windowSize"), 1, new float[] { Window.Width, Window.Height });
            GL.Uniform1(_shader.GetUniformLocation("u_zoom"), 1, new[] { (Window.Height * 0.5f) / (float)Math.Tan(90.0f * (Math.PI / 360.0f)) });
            GL.Uniform1(_shader.GetUniformLocation("f"), 1, new[] { f });
            
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            
            _shader.Unbind();
        }

        public override void OnDestroy()
        {
        }
    }
}
