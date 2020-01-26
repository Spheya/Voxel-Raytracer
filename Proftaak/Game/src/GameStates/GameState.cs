/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Gezien dit een van de belangrijkste bestanden is, laat ik hier een comment achter.
/// De game is jammergenoeg niet af, we hadden simpel weg niet genoeg tijd.
/// Basically alles is geimplemented, het enige wat nog moet is de gameplay. Alle backend hiervoor is aanwezig.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
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
using Networking;
using EntitySystem;
using Game.Gameplay;

namespace Game.GameStates
{
    sealed class GameState : ApplicationState
    {
        string Name;
        public GameState(string name)
        {
            Name = name;
        }
        private ulong _playerId = 0;
        private EntityManager _entityManager = new EntityManager();
        private PacketSender _packetSender = new PacketSender();

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
                Shader scaleFragmentShader = new Shader(ShaderType.FragmentShader, ShaderPreprocessor.Execute(@"res\shaders\raytracing\scale.glsl"));
                _voxelRenderer = new VoxelRenderer(new ShaderProgram(new[] { voxelVertexShader, voxelFragmentShader }), new ShaderProgram(new[] { voxelVertexShader, scaleFragmentShader }) );

                Shader spriteVertexShader = new Shader(ShaderType.VertexShader, ShaderPreprocessor.Execute(@"res\shaders\ui\vertex.glsl"));
                Shader spriteFragmentShader = new Shader(ShaderType.FragmentShader, ShaderPreprocessor.Execute(@"res\shaders\ui\fragment.glsl"));
                _spriteRenderer = new SpriteRenderer(new ShaderProgram(new[] { spriteVertexShader, spriteFragmentShader }));

            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            _voxelRenderer.GenerateFramebuffer(window);

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
            VoxReader r = new VoxReader(@"res\maps\map.vox", CastleVox);
            r.Read();

            //Use palette of castlevox
            List<Material> materials = new List<Material>();
            for (int i = 0; i < 256; i++)
            {
                Vector3 color = new Vector3((float)CastleVox._materials[i].r / 255f, (float)CastleVox._materials[i].g / 255f, (float)CastleVox._materials[i].b / 255f);
                float ior = 1.01f;
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


            //int s = 512;
            //VoxelModel model3 = _voxelRenderer.CreateModel(s,1,s,
            //    new Transform(new Vector3(0.0f, -48.0f, 0.0f), Vector3.Zero, new Vector3(1.0f)));

            //for (int x = 0; x < s; x++)
            //for (int y = 0; y < 1; y++)
            //for (int z = 0; z < s; z++)
            //    model3[x,y,z] = (byte)1;

            Console.WriteLine("Epic");

            Sprite crosshair = new Sprite(new Texture("res/textures/crosshair.png", TextureMinFilter.Linear, TextureMagFilter.Linear));
            _spriteRenderer.Add(crosshair);
            crosshair.Colour = new Colour(1.0f, 1.0f, 1.0f, 1.0f);
            crosshair.Transform.Scale = new Vector3(128.0f, 128.0f, 0.25f);

            List<DirectionalLight> dirLights = new List<DirectionalLight>();
            var sun = new DirectionalLight();
            sun.direction = new Vector3(-0.5f, 1.5f, -1.0f);
            sun.intensity = 0.8f;
            sun.colour = new Vector3(1f, 1f, 1f);
            dirLights.Add(sun);

            var sun2 = new DirectionalLight();
            sun2.direction = new Vector3(-0.4f, 1.5f, 0.8f);
            sun2.intensity = 0.6f;
            sun2.colour = new Vector3(0.7f, 0.8f, 1f);
            dirLights.Add(sun2);

            var sun3 = new DirectionalLight();
            sun3.direction = new Vector3(0.4f, 0.1f, 0.2f);
            sun3.intensity = 0.6f;
            sun3.colour = new Vector3(0.4f, 0.8f, 1f);
            dirLights.Add(sun3);
            _voxelRenderer.DirectionalLights = dirLights;

            List<PointLight> pointLights = new List<PointLight>();
            var pointlight = new PointLight();
            pointlight.position = new Vector3(0f, 0f, -20f);
            pointlight.intensity = 0.5f;
            pointlight.colour = new Vector3(1f, 0.0f, 0.0f);
            pointLights.Add(pointlight);
            _voxelRenderer.PointLights = pointLights;

            MyVoxLoader CharBodyVox = new MyVoxLoader();
            VoxReader rCharBody = new VoxReader(@"res\char1_body.vox", CharBodyVox);
            rCharBody.Read();

            //Use palette of castlevox
            //List<Material> materialsChar = new List<Material>();
            //for (int i = 0; i < 256; i++)
            //{
            //    Vector3 color = new Vector3((float)CharBodyVox._materials[i].r / 255f, (float)CharBodyVox._materials[i].g / 255f, (float)CharBodyVox._materials[i].b / 255f);
            //    float ior = 1.01f;
            //    if (i == 252) ior = 1.1f;
            //    if (i == 254) ior = 1.1f;
            //    //Vector3 color = new Vector3(1f, 0f, 0f);
            //    materialsChar.Add(new Material(color, ior));
            //}
            //_voxelRenderer.Materials.Set(materialsChar);

            TcpConnection connection = new TcpConnection(IPAddress.Parse("127.0.0.1"), 42069, (IConnection c, byte[] data) =>
            {
                Console.WriteLine("Received packet...");
                if(data[0] == 1)
                {
                    _playerId = BitConverter.ToUInt64(data, 1);
                } else if (data[0] == 0)
                {
                    if (NetworkEntity.HandlePacket(_entityManager, data, data[5]) == false)
                    {
                        //Player doesn't exist yet
                        Console.WriteLine("Adding new player");
                        Player player = new Player(data[1], data[5]);
                        player._modelBody = _voxelRenderer.CreateModel(CharBodyVox.Width, CharBodyVox.Height, CharBodyVox.Depth,
                        new Transform(new Vector3(-24.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.1f, 0.0f), new Vector3(0.5f)));

                        for (int x = 0; x < CharBodyVox.Width; x++)
                            for (int y = 0; y < CharBodyVox.Height; y++)
                                for (int z = 0; z < CharBodyVox.Depth; z++)
                                    player._modelBody[x, y, z] = CharBodyVox._data[x, y, z];
                        _entityManager.Add(player);
                    }
                }
            });

            _packetSender.AddReceiver(connection);
        }

        private float f = 0.0f;

        public override void OnUpdate(float deltatime)
        {
            f += deltatime;
            var pointlight = new PointLight();
            pointlight.position = new Vector3((float)Math.Sin(f) * 40.0f, 0.0f, -30.0f);
            pointlight.intensity = 2.0f;
            pointlight.colour = new Vector3(1f, 0.4f, 0.8f);
            _voxelRenderer.PointLights[0] = pointlight;

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
            _entityManager.FixedUpdate(deltatime);

            foreach(var entity in _entityManager.OfType<NetworkEntity>())
                _packetSender.EnqueuePacket(entity.GetPacket());

            _packetSender.Send();
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
