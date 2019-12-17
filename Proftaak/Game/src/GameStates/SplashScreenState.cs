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
using System.Threading;

namespace Game.GameStates
{
    class SplashScreenState : ApplicationState
    {
        const float BEGIN_TIME = 0.2f;
        const float SPEED_FADE = 1.5f;

        float _count = 0;
        float _fade = 1.0f;

        SpriteRenderer _renderer;
        Sprite _sprite1;
        Transform _transform1;

        public override void OnCreate()
        {
            try
            {
                Shader spriteVertexShader = new Shader(ShaderType.VertexShader, ShaderPreprocessor.Execute(@"res\shaders\ui\vertex.glsl"));
                Shader spriteFragmentShader = new Shader(ShaderType.FragmentShader, ShaderPreprocessor.Execute(@"res\shaders\ui\fragment.glsl"));
                _renderer = new SpriteRenderer(new ShaderProgram(new[] { spriteVertexShader, spriteFragmentShader }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            Vector3 vector1 = new Vector3(20, 30, 30);
            _transform1 = new Transform(vector1, Vector3.Zero, new Vector3(window.Width, window.Height, 30));
            Texture texture1 = new Texture("res\\textures\\splashscreen.png");
            _sprite1 = new Sprite(texture1);
        }


        public override void OnDestroy()
        {
        }
        public override void OnDraw(float deltatime)
        {
            _renderer.Draw(window);
        }

        public override void OnFixedUpdate(float deltatime)
        {
        }

        public override void OnUpdate(float deltatime)
        {
            _count += deltatime;
            if (_count >= BEGIN_TIME) {
                _fade -= deltatime * SPEED_FADE;

                if (_fade <= 0.0f)
                {
                    RequestState(new MenuState());
                }
            }

            Colour color1 = new Colour(_fade, _fade, _fade);
            _sprite1.Colour = color1;
            _renderer.Add(_sprite1);
            _sprite1.Transform = _transform1;
;        }
    }
}
