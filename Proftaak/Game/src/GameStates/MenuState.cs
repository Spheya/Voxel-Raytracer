using System;
using Game.Engine.Rendering;
using Game.Engine.Shaders;
using Game.UI;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Game.Engine.Maths;
using Game.Engine.Input;
using OpenTK.Input;
using System.Collections.Generic;

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
        Button _playbutton2;
        Button _playbutton3;
        Button _playbutton4;
        List<Button> buttons = new List<Button>();
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
            _playbutton = new Button(texture1, new Transform(new Vector3(200, 200, 0), Vector3.Zero, new Vector3(window.Width / 2, window.Height / 2, 30)));
            _playbutton2 = new Button(texture1, new Transform(new Vector3(300, 300, 0), Vector3.Zero, new Vector3(window.Width / 2, window.Height / 2, 30)));
            _playbutton3 = new Button(texture1, new Transform(new Vector3(400, 400, 0), Vector3.Zero, new Vector3(window.Width / 2, window.Height / 2, 30)));
            _playbutton4 = new Button(texture1, new Transform(new Vector3(0, 0, 0), Vector3.Zero, new Vector3(window.Width / 2, window.Height / 2, 30)));

            for (int i = 0; i > -200; i-=100)
            {
                _playbutton = new Button(texture1, new Transform(new Vector3(i, i, 0), Vector3.Zero, new Vector3(window.Width / 2, window.Height / 2, 30)));
                _playbutton.AddToRenderer(_renderer);
                buttons.Add(_playbutton);
            }
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
            ButtonState Mouse;
            _playbutton.Update();
            MouseInput.Update();
            Vector2 test = MouseInput.GetMousePos();
            Mouse = MouseInput.GetMouseLeftButton();
            if (test.X > (buttons[0].GetPosition().X + 1f) * window.Width / 2f - buttons[0].GetSize().X / 2 && test.X < (buttons[0].GetPosition().X + 1f) * window.Width / 2f + buttons[0].GetSize().X / 2 && test.Y < (buttons[0].GetPosition().Y + 1f) * window.Height / 2f + buttons[0].GetSize().Y / 2 && test.Y > (buttons[0].GetPosition().Y + 1f) * window.Height / 2f - buttons[0].GetSize().Y / 2)
            {
                if(Mouse == ButtonState.Pressed)
                {
                    Console.WriteLine("1");
                }
                else
                {
                    Console.WriteLine("2");
                }
            }
            else
            {
                Console.WriteLine("3");
            }
        }
    }
}