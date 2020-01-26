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
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Diagnostics;
using Game.src.UI;

namespace Game.GameStates
{
    class MenuState : ApplicationState
    {
        Text info = new Text();
        public static SpriteRenderer _renderer;
        Sprite _background;
        Button _playbutton;
        Button _playbutton2;
        Button _playbutton3;
        Button _playbutton4;
        Sprite buttonBackground;
        List<Button> buttons = new List<Button>();
        public static string sayString = "Dit is onze renderer. Veel plezier.";
        public override void OnCreate()
        {
            //Make WinImage
            //Process cmd = new Process();
            //cmd.StartInfo.FileName = @"C:\Users\mikel\Desktop\runFIRST.cmd";
            //cmd.StartInfo.RedirectStandardInput = true;
            //cmd.StartInfo.RedirectStandardOutput = true;
            //cmd.StartInfo.CreateNoWindow = true;
            //cmd.StartInfo.UseShellExecute = false;
            //cmd.Start();

            //Get usernames
            //Text.FontList();
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
            Texture texture1 = new Texture("res\\textures\\CustomButton1.png");
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
            int toBelow = 50;
            _playbutton = new Button(texture1, new Transform(new Vector3(-window.Width / gedeeldDoor, -window.Height / gedeeldDoor2-toBelow, 0), Vector3.Zero, new Vector3(buttonWidth, buttonHeight, 0)), color2);
            _playbutton2 = new Button(texture1, new Transform(new Vector3(window.Width / gedeeldDoor, -window.Height / gedeeldDoor2-toBelow, 0), Vector3.Zero, new Vector3(buttonWidth, buttonHeight, 0)));
            _playbutton3 = new Button(texture1, new Transform(new Vector3(-window.Width / gedeeldDoor, window.Height / gedeeldDoor2-toBelow, 0), Vector3.Zero, new Vector3(buttonWidth, buttonHeight, 0)), color4);
            _playbutton4 = new Button(texture1, new Transform(new Vector3(window.Width / gedeeldDoor, window.Height / gedeeldDoor2-toBelow, 0), Vector3.Zero, new Vector3(buttonWidth, buttonHeight, 0)), color3);
            Colour colorBackground = new Colour(0, 0, 255);
            Texture textureBackground = texture2;
            Transform transformationBackground = new Transform(new Vector3(-window.Width/2+225, window.Height / gedeeldDoor2 + 130, 0), Vector3.Zero, new Vector3(buttonWidth, buttonHeight/4, 0));
            buttonBackground = new Sprite(textureBackground, transformationBackground, colorBackground);
            _renderer.Add(buttonBackground);
            buttons.Add(_playbutton);
            buttons.Add(_playbutton2);
            buttons.Add(_playbutton3);
            buttons.Add(_playbutton4);
            foreach (Button button in buttons)
            {
                //Console.WriteLine(button);
                button.AddToRenderer(_renderer);
            }
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
            if (!string.IsNullOrEmpty(sayString))
            {
                //Adds text that sayString holds
                string[] sayStringSplit = sayString.Split(' ');
                int verPos = 0;
                int newdit = 0;
                int totLenght = 0;
                foreach (string word in sayStringSplit)
                {
                    info.AddStringText(word, window, verPos);
                    totLenght += word.Length;
                    if (totLenght > 5)
                    {
                        verPos += 15;
                        info.posSayString = 0;
                        totLenght = 0;
                    }
                    else
                    {
                        info.AddStringText(" ", window, verPos);
                    }
                }
            }
        }
        private void _playbutton_OnClick(object sender, EventArgs e)
        {
            Console.WriteLine("1");
        }
        private void _playbutton2_OnClick(object sender, EventArgs e)
        {
            Console.WriteLine("2");
            //UDP.dit();
        }
        private void _playbutton3_OnClick(object sender, EventArgs e)
        {
            Console.WriteLine("3");
            //Console.WriteLine(info.textString);
        }
        
        private void _playbutton4_OnClick(object sender, EventArgs e)
        {
            //new Window(new GameState()).Run();
            RequestState(new GameState(Convert.ToString(info.textString)));
            //RequestState(new GameState());
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
        int CW;
        int CH;
        public override void OnUpdate(float deltatime)
        {
            int ConsoleWidth = window.Width;
            int ConsoleHeight = window.Height;
            if (CW != ConsoleWidth || CH != ConsoleHeight)
            {
                Console.WriteLine("Window Size Changed");
                Transform transform1 = new Transform(new Vector3(0, 0, 0), Vector3.Zero, new Vector3(window.Width, window.Height, 30));
                Texture texture1 = new Texture("res\\textures\\CustomBackground.png");
                _renderer.Remove(_background);
                _background = new Sprite(texture1, transform1);
                _renderer.Add(_background);
                foreach (Button button in buttons)
                {
                    button.RemoveToRenderer(_renderer);
                    button.AddToRenderer(_renderer);
                }
                _renderer.Remove(buttonBackground);
                _renderer.Add(buttonBackground);
                foreach (Sprite Font in info.TextArray)
                {
                    MenuState._renderer.Remove(Font);
                    MenuState._renderer.Add(Font);
                }
            }
            CW = ConsoleWidth;
            CH = ConsoleHeight;
            bool TextCount = KeyboardInput.IsAnyDown();
            if (NewTextCount != TextCount)
            {
                if (NewTextCount == false)
                {
                    info.AddText();
                }
            }
            NewTextCount = TextCount;
            KeyboardInput.Update();
            foreach (Button button in buttons)
            {
                button.Update(window);
            }
        }
    }
}