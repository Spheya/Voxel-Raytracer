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
using Game.Engine;
using Game.GameStates;
using System.Windows.Documents;
using System.Net;
using System.IO;
using Networking.src;

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
        Button _playbutton2;
        Button _playbutton3;
        Button _playbutton4;
        List<Button> buttons = new List<Button>();
        List<string> fontPictures = new List<string>();
        public override void OnCreate()
        {
            string[] allFiles = Directory.GetFiles(@"res\font\");
            foreach (string file in allFiles)
            {
                Console.WriteLine(file);
                fontPictures.Add(file);
            }
            //var a = Dns.GetHostEntry(Dns.GetHostName());
            //Console.WriteLine(a.HostName);
            //var b = a.AddressList;
            //foreach (var adress in b)
            //{
            //    Console.WriteLine(adress);
            //}
            //List<string> usernames = new List<string>();
            //string text = File.ReadAllText(@"res\variables.txt");
            //string[] splitUsernames = text.Split(',');
            //foreach (string username in splitUsernames)
            //{
            //    Console.WriteLine(username);
            //    usernames.Add(username);
            //}
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
            Texture texture2 = new Texture("res\\textures\\babyyodasoup.png");
            Texture texture3 = new Texture("res\\textures\\yodawithsword.png");
            Colour color1 = new Colour(1, 1, 1);
            Colour color2 = new Colour(0, 0, 255);
            Colour color3 = new Colour(255, 0, 0, .5f);
            Colour color4 = new Colour(0, 255, 0, .5f);
            _background = new Sprite(texture1, transform1);
            _background.Colour = color1;
            _renderer.Add(_background);
            int buttonWidth = 400;
            int buttonHeight = 200;
            int gedeeldDoor = 5;
            int gedeeldDoor2 = 5;
            _playbutton = new Button(texture1, new Transform(new Vector3(-window.Width / gedeeldDoor, -window.Height / gedeeldDoor2, 0), Vector3.Zero, new Vector3(buttonWidth, buttonHeight, 0)), color2);
            _playbutton2 = new Button(texture1, new Transform(new Vector3(window.Width / gedeeldDoor, -window.Height / gedeeldDoor2, 0), Vector3.Zero, new Vector3(buttonWidth, buttonHeight, 0)));
            _playbutton3 = new Button(texture1, new Transform(new Vector3(-window.Width / gedeeldDoor, window.Height / gedeeldDoor2, 0), Vector3.Zero, new Vector3(buttonWidth, buttonHeight, 0)), color4);
            _playbutton4 = new Button(texture1, new Transform(new Vector3(window.Width / gedeeldDoor, window.Height / gedeeldDoor2, 0), Vector3.Zero, new Vector3(buttonWidth, buttonHeight, 0)), color3);
            buttons.Add(_playbutton);
            buttons.Add(_playbutton2);
            buttons.Add(_playbutton3);
            buttons.Add(_playbutton4);
            buttons.Add(new Button(texture1, new Transform(new Vector3(-window.Width * 0.5f, -window.Height * 0.5f, 0.0f), Vector3.Zero, Vector3.One * 64.0f)));

            foreach (Button button in buttons)
            {
                //Console.WriteLine(button);
                button.AddToRenderer(_renderer);
            }
            
            //ButtonAdder();
            _playbutton.OnClick += _playbutton_OnClick;
            _playbutton2.OnClick += _playbutton2_OnClick;
            _playbutton3.OnClick += _playbutton3_OnClick;
            _playbutton4.OnClick += _playbutton4_OnClick;
            //foreach (string font in fontPictures)
            //{
            //    Texture textureFont = new Texture(font);
            //    AddFont(i, 0, textureFont);
            //    i += 200;
            //}
        }
        private void _playbutton_OnClick(object sender, EventArgs e)
        {

        }
        void AddFont(int x, int y, Texture texture)
        {
            int dit = 7;
            Colour colorFont = new Colour(1, 1, 1);
            Transform transformFont = new Transform(new Vector3(x, y, 0), Vector3.Zero, new Vector3(1000 / dit, 500 / dit, 0));
            Sprite Font = new Sprite(texture, transformFont, colorFont);
            _renderer.Add(Font);
        }
        private void _playbutton2_OnClick(object sender, EventArgs e)
        {
            Console.WriteLine("3");
            //UDP.dit();
        }
        private void _playbutton3_OnClick(object sender, EventArgs e)
        {
            Console.WriteLine("1");
            //bool currentKeyboardState = KeyboardInput.UpdateReturn();
            //Console.WriteLine(currentKeyboardState);
        }
        int i = 0;
        private void AddText()
        {
            int i2 = i - window.Width/2;
            //TODO: Change to isKeyDown(Key key)
            bool M = KeyboardInput.IsMDown();
            bool I = KeyboardInput.IsIDown();
            bool K = KeyboardInput.IsKDown();
            bool E = KeyboardInput.IsEDown();
            bool Backspace = KeyboardInput.IsBackspaceDown();
            Texture textureFont;
            if (Backspace == true)
            {
                textureFont = new Texture(fontPictures[fontPictures.IndexOf("res\\font\\M.png")]);
                AddFont(i2, 0, textureFont);
                i += 200;
            }
            if (M == true)
            {
                textureFont = new Texture(fontPictures[fontPictures.IndexOf("res\\font\\M.png")]);
                AddFont(i2+1000/7, 0, textureFont);
                i += 200;
            }
            if (I == true)
            {
                textureFont = new Texture(fontPictures[fontPictures.IndexOf("res\\font\\I.png")]);
                AddFont(i2, 0, textureFont);
                i += 200;
            }
            if (K == true)
            {
                textureFont = new Texture(fontPictures[fontPictures.IndexOf("res\\font\\K.png")]);
                AddFont(i2, 0, textureFont);
                i += 200;
            }
            if (E == true)
            {
                textureFont = new Texture(fontPictures[fontPictures.IndexOf("res\\font\\E.png")]);
                AddFont(i2, 0, textureFont);
                i += 200;
            }
        }
        private void _playbutton4_OnClick(object sender, EventArgs e)
        {
            //new Window(new GameState()).Run();
            RequestState(new GameState());
            //new Window(new SplashScreenState()).Run();
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
        bool NewTextCount=false;
        public override void OnUpdate(float deltatime)
        {
            bool TextCount = KeyboardInput.IsAnyDown();
            if (NewTextCount != TextCount)
            {
                AddText();
            }
            NewTextCount = TextCount;
            KeyboardInput.Update();
            ButtonState MouseLeft = MouseInput.GetMouseLeftButton();
            foreach (Button button in buttons)
            {
                button.Update(window);
            }
        }
    }
}