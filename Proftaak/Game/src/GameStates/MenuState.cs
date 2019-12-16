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
    class MenuState : ApplicationState
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
        //Button _playbutton2;
        //Button _playbutton3;
        //Button _playbutton4;
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
            Colour color1 = new Colour(1, 1, 1);
            Colour color2 = new Colour(0, 0, 255, .5f);
            _background = new Sprite(texture1, transform1);
            _background.Colour = color1;
            _renderer.Add(_background);
            _playbutton = new Button(texture1, new Transform(new Vector3(-window.Width / 4, -window.Height / 4, 0), Vector3.Zero, new Vector3(window.Width / 2, window.Height / 2, 0)), color2);
            //_playbutton2 = new Button(texture1, new Transform(new Vector3(300, 300, 0), Vector3.Zero, new Vector3(window.Width / 2, window.Height / 2, 30)));
            //_playbutton3 = new Button(texture1, new Transform(new Vector3(400, 400, 0), Vector3.Zero, new Vector3(window.Width / 2, window.Height / 2, 30)));
            //_playbutton4 = new Button(texture1, new Transform(new Vector3(0, 0, 0), Vector3.Zero, new Vector3(window.Width / 2, window.Height / 2, 30)));
            buttons.Add(_playbutton);
            Console.WriteLine("Button List: ");
            foreach (Button button in buttons)
            {
                Console.WriteLine(button);
                button.AddToRenderer(_renderer);
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
            _playbutton.Update();
            MouseInput.Update();
            Vector2 MousePos = MouseInput.GetMousePos();
            ButtonState MouseLeft = MouseInput.GetMouseLeftButton();
            Button a = _playbutton;
            //Console.WriteLine(MousePos.X);
            //kijkt of het tussen de x-waardes zit (buitendste deel)
            if (MousePos.X > a.GetPosition().X / 2f + window.Width / 2 - a.GetSize().X / 2f &&
                MousePos.X < a.GetPosition().X / 2f + window.Width / 2 + a.GetSize().X / 2f &&
                MousePos.Y < -a.GetPosition().Y / 2f + window.Height / 2 + a.GetSize().Y / 2f &&
                MousePos.Y > -a.GetPosition().Y / 2f + window.Height / 2 - a.GetSize().Y / 2f)
            {
                Console.WriteLine("werkt");
            }
            else
            {
                Console.WriteLine("werkt niet");
            }
                //kijkt of het tussen de y-waardes zit
                //if (MousePos.Y > a.GetPosition().Y / 2 + window.Width / 2 - a.GetSize().Y / 2 && MousePos.X < a.GetPosition().X + window.Width / 2 + a.GetSize().X / 2)
                //{
                //    Console.WriteLine("in alles");
                //}
                //else
                //{
                //    Console.WriteLine("buiten Y");
                //}
                //Console.WriteLine("in X");
            }
            //else
            //{
            //    Console.WriteLine("buiten X");
            //}
        }
    }