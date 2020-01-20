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
        public static SpriteRenderer _renderer;
        Sprite _background;
        Button _playbutton;
        Button _playbutton2;
        Button _playbutton3;
        Button _playbutton4;
        List<Button> buttons = new List<Button>();
        public static string sayString = "Test";
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
            Texture texture1 = new Texture("res\\textures\\Button.png");
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
                //double RowCount = sayString.Length / 20f;
                //RowCount = Math.Ceiling(RowCount);
                //Console.WriteLine(RowCount);
                //int i = 0;
                //int tijdelijk = 0;
                //for (int VerPos = 0; i < RowCount; VerPos += 15)
                //{
                //    var normal = sayString.Substring(tijdelijk, 20);
                //    if (i == 5)
                //    {
                //        normal = sayString.Substring(tijdelijk);
                //    }
                //    //Console.WriteLine(VerPos);
                //    Console.WriteLine(tijdelijk);
                //Text.AddStringText(normal, window, VerPos);
                Text.AddStringText(sayString, window, 0);
                //    tijdelijk += 20;
                //    i++;
                //    Text.posSayString = 0;
                //}
            }
        }
        private void _playbutton_OnClick(object sender, EventArgs e)
        {
            Console.WriteLine("1");
        }
        private void _playbutton2_OnClick(object sender, EventArgs e)
        {
            Console.WriteLine("2");
            UDP.dit();
        }
        private void _playbutton3_OnClick(object sender, EventArgs e)
        {
            Console.WriteLine("3");
            //bool currentKeyboardState = KeyboardInput.UpdateReturn();
            //Console.WriteLine(currentKeyboardState);
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
                Texture texture1 = new Texture("res\\textures\\background2.png");
                _renderer.Remove(_background);
                _background = new Sprite(texture1, transform1);
                _renderer.Add(_background);
                _playbutton.AddToRenderer(_renderer);
                _playbutton2.AddToRenderer(_renderer);
                _playbutton3.AddToRenderer(_renderer);
                _playbutton4.AddToRenderer(_renderer);
                foreach (Sprite Font in Text.TextArray)
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
                    Text.AddText();
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