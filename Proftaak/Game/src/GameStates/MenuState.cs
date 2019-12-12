using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Rendering;
using Game.Engine.Shaders;
using Game.UI;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Game.Engine.Maths;
using System.Threading;
using System.Drawing;
using Game.Engine.Input;

namespace Game.GameStates
{
    class MenuState:ApplicationState
    {
        //public void ButtonAdder()
        //{
        //    Transform _transform2 = new Transform(vector1, Vector3.Zero, new Vector3(window.Width/2, window.Height/2, 30));
        //    _sprite2 = new Sprite(texture1);
        //    _sprite2.Colour = color1;
        //    _sprite2.Transform = _transform2;
        //    _renderer.Add(_sprite2);
        //}
        SpriteRenderer _renderer;
        Sprite _background;
        Button _playbutton;
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
            
            Transform transform1 = new Transform(new Vector3(0, 0, 0), Vector3.Zero, new Vector3(window.Width, window.Height, 30));
            Texture texture1 = new Texture("res\\textures\\mini_yoda.png");
            _background = new Sprite(texture1, transform1);
            Colour color1 = new Colour(1, 1, 1);
            _background.Colour = color1;
            _renderer.Add(_background);
            _playbutton = new Button(texture1, new Transform(new Vector3(0, 0, 0), Vector3.Zero, new Vector3(window.Width/2, window.Height/2, 30)));
            _playbutton.AddToRenderer(_renderer);
            //ButtonAdder();
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
            _playbutton.Update();
            MouseInput.Update();
            Vector2 test = MouseInput.GetMousePos();
            //Console.WriteLine(test.X);
        }
    }
}