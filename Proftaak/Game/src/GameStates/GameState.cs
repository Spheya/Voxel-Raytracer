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
        private Renderer _renderer;

        private VoxelModel _model;

        private ShaderProgram _shader;

        public override void OnCreate()
        {
            try
            {
                Shader vertexShader = new Shader(ShaderType.VertexShader, 
                    ShaderPreprocessor.Execute(new[] { "TEST_COLOR" }, File.ReadAllLines(@"res\vertex3d.glsl"), @"res\"));
                Shader fragmentShader = new Shader(ShaderType.FragmentShader, 
                    ShaderPreprocessor.Execute(new[] { "TEST_COLOR" }, File.ReadAllLines(@"res\fragment.glsl"), @"res\"));

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
            _model.Transform.Position = Vector3.One;
  
            _renderer = new Renderer();
            _renderer.Add(_model);

            Console.WriteLine("Epic");
        }

        public override void OnUpdate(float deltatime)
        {
            Random rand = new Random();
            _model[rand.Next(_model.Width), rand.Next(_model.Height), rand.Next(_model.Depth)] = new Voxel(1);

            _model.Transform.Rotation += new Vector3(0.31243f, 0.3764356f, 0.455f) * deltatime;
        }

        public override void OnFixedUpdate(float deltatime)
        {
        }

        public override void OnDraw(float deltatime)
        {
            _renderer.Draw(_shader, new Camera(new Vector3(16, 16, 64), Vector3.Zero), Window);
        }

        public override void OnDestroy()
        {
        }
    }
}
