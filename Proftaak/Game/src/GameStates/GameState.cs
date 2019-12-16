using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsharpVoxReader;
using Game.Engine.Input;
using Game.Engine.Maths;
using Game.Engine.Rendering;
using Game.Engine.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using VoxelData;
using VoxLoader;

namespace Game.GameStates
{
    sealed class GameState : ApplicationState
    {

        private readonly FreeCamera _camera = new FreeCamera(new Vector3(0.0f, 0.0f, -32.0f), new Vector3(0.0f, 0.0f, 0.0f));

        private VoxelRenderer _voxelRenderer;
        private SpriteRenderer _spriteRenderer;

        private VoxelModel _model;
        private VoxelModel _model2;
        public override void OnCreate()
        {
            try
            {
                
                Console.WriteLine(ShaderPreprocessor.Execute(@"res\shaders\raytracing\fragment.glsl"));

                Shader voxelVertexShader = new Shader(ShaderType.VertexShader, ShaderPreprocessor.Execute(@"res\shaders\raytracing\vertex.glsl"));
                Shader voxelFragmentShader = new Shader(ShaderType.FragmentShader, ShaderPreprocessor.Execute(@"res\shaders\raytracing\fragment.glsl"));
                _voxelRenderer = new VoxelRenderer(new ShaderProgram(new[] { voxelVertexShader, voxelFragmentShader }));

                Shader spriteVertexShader = new Shader(ShaderType.VertexShader, ShaderPreprocessor.Execute(@"res\shaders\ui\vertex.glsl"));
                Shader spriteFragmentShader = new Shader(ShaderType.FragmentShader, ShaderPreprocessor.Execute(@"res\shaders\ui\fragment.glsl"));
                _spriteRenderer = new SpriteRenderer(new ShaderProgram(new[] { spriteVertexShader, spriteFragmentShader }));

            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            //List<Material> materials = new List<Material>();
            //for (int i = 0; i < 256; i++)
            //    materials.Add(new Material(Vector3.One, 1.5f));

            //_voxelRenderer.Materials = materials;

            Console.WriteLine("Shader compiled <o/"); //epic it work

            _model = _voxelRenderer.CreateModel(32, 32, 32,
                new Transform(new Vector3(24.0f, 0.0f, 0.0f), Vector3.Zero, new Vector3(0.5f)));

            for (int x = 0; x < 32; x++)
            for (int y = 0; y < 32; y++)
            for (int z = 0; z < 32; z++)
                _model[x, y, z] = (byte)((x+y+z)&1);//new Voxel((ushort) ((x + y + z) & 1));

            MyVoxLoader CastleVox = new MyVoxLoader();
            VoxReader r = new VoxReader(@"res\maps\monu10.vox", CastleVox);
            r.Read();

            //Use palette of castlevox
            List<Material> materials = new List<Material>();
            for (int i = 0; i < 256; i++)
            {
                Vector3 color = new Vector3((float)CastleVox._materials[i].r / 255f, (float)CastleVox._materials[i].g / 255f, (float)CastleVox._materials[i].b / 255f);
                float ior = 0f;
                if (i == 252) ior = 1.1f;
                if (i == 254) ior = 1.1f;
                //Vector3 color = new Vector3(1f, 0f, 0f);
                materials.Add(new Material(color, ior));
            }
            _voxelRenderer.Materials.Set(materials);

            _model2 = _voxelRenderer.CreateModel(CastleVox.Width, CastleVox.Height, CastleVox.Depth,
                new Transform(new Vector3(-24.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.1f, 0.0f), new Vector3(0.5f)));

            //for (int x = -16; x < 16; x++)
            //for (int y = -16; y < 16; y++)
            //for (int z = -16; z < 16; z++)
            //    _model2[x + 16, y + 16, z + 16] = (x * x + y * y + z * z < 16 * 16) ? (byte)1 : (byte)0;
            for (int x = 0; x < CastleVox.Width; x++)
            for (int y = 0; y < CastleVox.Height; y++)
            for (int z = 0; z < CastleVox.Depth; z++)
                _model2[x,y,z] = CastleVox._data[x,y,z];

            Console.WriteLine("Epic");


            int s = 512;
            VoxelModel model3 = _voxelRenderer.CreateModel(s,1,s,
                new Transform(new Vector3(0.0f, -48.0f, 0.0f), Vector3.Zero, new Vector3(1.0f)));

            for (int x = 0; x < s; x++)
            for (int y = 0; y < 1; y++)
            for (int z = 0; z < s; z++)
                model3[x,y,z] = (byte)1;

            Console.WriteLine("Epic");

            Sprite crosshair = new Sprite(new Texture("res/textures/crosshair.png", TextureMinFilter.Linear, TextureMagFilter.Linear));
            _spriteRenderer.Add(crosshair);
            crosshair.Colour = new Colour(1.0f, 1.0f, 1.0f, 1.0f);
            crosshair.Transform.Scale = new Vector3(128.0f, 128.0f, 0.25f);

            //List<DirectionalLight> dirLights = new List<DirectionalLight>();
            //var sun = new DirectionalLight();
            //sun.direction = new Vector3(-0.5f, 1.5f, -1.0f);
            //sun.intensity = 1f;
            //sun.colour = new Vector3(1f, 1f, 1f);
            //dirLights.Add(sun);
            //_voxelRenderer._dirLights = dirLights;

            List<PointLight> pointLights = new List<PointLight>();
            var pointlight = new PointLight();
            pointlight.position = new Vector3(0f, 0f, -20f);
            pointlight.intensity = 5f;
            pointlight.colour = new Vector3(1f, 1f, 1f);
            pointLights.Add(pointlight);
            _voxelRenderer._pointLights = pointLights;
        }

        public override void OnUpdate(float deltatime)
        {
            window.CursorVisible = !window.Focused;
            if (!window.CursorVisible)
                Mouse.SetPosition(window.X + window.Width * 0.5, window.Y + window.Height * 0.5);

            _model.Transform.Rotation += new Vector3(deltatime, deltatime, deltatime);
            //_model2.Transform.Rotation -= new Vector3(0.0f, deltatime * 0.25f, 0.0f);

            //Console.WriteLine(_model.Transform.Rotation);

            KeyboardInput.Update();
            MouseInput.Update();
            //Random rand = new Random();
            //_model[rand.Next(_model.Width), rand.Next(_model.Height), rand.Next(_model.Depth)] = new Voxel(1);
            _camera.Update(deltatime);

            //Console.WriteLine(_model.Transform.CalculateInverseMatrix().Column0);
        }

        public override void OnFixedUpdate(float deltatime)
        {
        }

        public override void OnDraw(float deltatime)
        {
           _voxelRenderer.Draw(_camera, window);
           _spriteRenderer.Draw(window);
        }

        public override void OnDestroy()
        {
        }
    }
}
