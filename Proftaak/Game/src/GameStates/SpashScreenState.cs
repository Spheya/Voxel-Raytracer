using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Rendering;
using Game.Engine.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Game.Engine.Maths;

namespace Game.GameStates
{
    class SpashScreenState : ApplicationState
    {
        SpriteRenderer renderer;

        public override void OnCreate()
        {
            try
            {
                Shader spriteVertexShader = new Shader(ShaderType.VertexShader, ShaderPreprocessor.Execute(@"res\shaders\ui\vertex.glsl"));
                Shader spriteFragmentShader = new Shader(ShaderType.FragmentShader, ShaderPreprocessor.Execute(@"res\shaders\ui\fragment.glsl"));
                renderer = new SpriteRenderer(new ShaderProgram(new[] { spriteVertexShader, spriteFragmentShader }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            Texture texture;
            Vector3 vector1 = new Vector3(20, 30, 30);
            Transform transform1 = new Transform(vector1, Vector3.Zero, new Vector3(System.Windows.SystemParameters.PrimaryScreenWidth, window.Height, 30));
            float kleur = 0.5F;
            Colour kleurdit = new Colour(0, 255, 0);
            Texture texture1 = new Texture("res\\textures\\crosshair.png");
            Sprite sprite1 = new Sprite(texture1);
            renderer.Add(sprite1);
            sprite1.Colour = kleurdit;
            sprite1.Transform = transform1;
        }

        public override void OnDestroy()
        {
        }
        public override void OnDraw(float deltatime)
        {
            renderer.Draw(window);
        }

        public override void OnFixedUpdate(float deltatime)
        {
        }

        public override void OnUpdate(float deltatime)
        {
        }
    }
}
