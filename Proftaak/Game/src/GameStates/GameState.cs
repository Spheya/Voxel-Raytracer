using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Input;
using Game.Engine.Rendering;
using Game.Engine.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using VoxelData;

namespace Game.GameStates
{
    sealed class GameState : ApplicationState
    {

        private FreeCamera _camera = new FreeCamera(new Vector3(256f, 256f, -270.0f), new Vector3(0.0f, 0.0f, 0.0f));

        private Renderer _renderer;

        public override void OnCreate()
        {
            try
            {
                Shader vertexShader = new Shader(ShaderType.VertexShader,
                    ShaderPreprocessor.Execute(null, File.ReadAllLines(@"res\vertex.glsl"), @"res\"));
                Shader fragmentShader = new Shader(ShaderType.FragmentShader,
                    ShaderPreprocessor.Execute(null, File.ReadAllLines(@"res\fragment.glsl"), @"res\"));

                _renderer = new Renderer(new ShaderProgram(new[] { vertexShader, fragmentShader }));
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            Console.WriteLine("Shader compiled <o/"); //epic it work

            VoxelModel model = _renderer.CreateModel(512, 512, 512);
            for (int x = 0; x < 512; x++)
                for (int y = 0; y < 512; y++)
                    for (int z = 0; z < 512; z++)
                    {
                        //int cx = x - 32; int cy = y - 32; int cz = z - 32;
                        //model[x, y, z] = new Voxel((ushort)((Math.Sqrt(cx * cx + cy * cy + cz * cz) < 32) ? 1 : 0));
                        model[x, y, z] = new Voxel((ushort)((x + y + z) & 1));
                    }
                //model[x,y,z] = new Voxel((ushort) ((x + y + z)&1));

            Console.WriteLine("Epic");
        }

        public override void OnUpdate(float deltatime)
        {
            KeyboardInput.Update();
            //Random rand = new Random();
            //_model[rand.Next(_model.Width), rand.Next(_model.Height), rand.Next(_model.Depth)] = new Voxel(1);
            _camera.Update(deltatime);
        }

        public override void OnFixedUpdate(float deltatime)
        {
        }

        public override void OnDraw(float deltatime)
        {
           _renderer.Draw(_camera, window);
        }

        public override void OnDestroy()
        {
        }
    }
}
