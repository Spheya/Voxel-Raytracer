using Game.Engine.Input;
using Game.Engine.Maths;
using Game.Engine.Rendering;
using Game.GameStates;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.src.UI
{
    class Text
    {
        public enum keys
        {
            A = 83,
            B = 84,
            C = 85,
            D = 86,
            E = 87,
            F = 88,
            G = 89,
            H = 90,
            I = 91,
            J = 92,
            K = 93,
            L = 94,
            M = 95,
            N = 96,
            O = 97,
            P = 98,
            Q = 99,
            R = 100,
            S = 101,
            T = 102,
            U = 103,
            V = 104,
            W = 105,
            X = 106,
            Y = 107,
            Z = 108,
            Space = 109
        }
        static Texture texture;
        static int o = 0;
        //static List<Texture> fontPictures = new List<Texture>();
        public static void FontList()
        {
            foreach (var item in OwnList)
            {
                //string[] allFiles = Directory.GetFiles(@"res\font\");
                //foreach (string file in allFiles)
                //{
                //    Console.WriteLine(file);
                //    fontPictures.Add(file);
                //}
                Image img = Image.FromFile(@"res\APdBm.png");
                int where = -1 * img.Width / 90 * o;
                Console.WriteLine(where);
                Rectangle r = new Rectangle(where, 0, img.Width, img.Height);
                Bitmap target = new Bitmap(img.Width / 90, img.Height);

                using (Graphics graphics = Graphics.FromImage(target))
                {
                    graphics.DrawImage(
                        img,
                        r);
                }
                //Texture tex = new Texture(target);
                //fontPictures.Add(tex);
                target.Save(@"res\Keys\" + item + ".png");
                o++;
            }
        }
        static List<keys> OwnList = Enum.GetValues(typeof(keys)).Cast<keys>().ToList();
        static List<Key> theList = Enum.GetValues(typeof(Key)).Cast<Key>().ToList();
        static int PosText = 0;
        static List<Sprite> typedText = new List<Sprite>();
        public static int posSayString = 0;
        public static void AddStringText(string sayString, GameWindow window, int VerPos)
        {
            foreach (char letter in sayString.ToUpper())
            {
                Texture texture222 = new Texture(@"res\Keys\" + letter + ".png");
                AddFont(posSayString - 780, window.Height / 2 - 15 - VerPos, texture222);
                posSayString += 25;
            }
        }
        public static void AddText()
        {
            foreach (var item in theList)
            {
                var itemDown = KeyboardInput.IsKeyDown(item);
                if (itemDown == true)
                {
                    if (item == Key.BackSpace)
                    {
                        MenuState._renderer.Remove(typedText.Last());
                        typedText.RemoveAt(typedText.Count() - 1);
                        PosText -= 25;
                        break;
                    }
                    else
                    {
                        //Console.WriteLine(item);
                        texture = new Texture(@"res\Keys\" + item + ".png");
                        Console.WriteLine(@"res\Keys\" + item + ".png");
                        AddFont(PosText, 0, texture);
                        PosText += 25;
                        break;
                    }
                }
            }
            //var M = KeyboardInput.IsKeyDown(Key.M);
            //if (M == true)
            //{
            //    PosText += 25;
            //    AddFont(PosText, 0, TextureM);
            //}
        }
        public static List<Sprite> TextArray = new List<Sprite>();
        static void AddFont(int x, int y, Texture texture)
        {
            int dit = 40;
            Colour colorFont = new Colour(1, 1, 1);
            Transform transformFont = new Transform(new Vector3(x, y, 0), Vector3.Zero, new Vector3(1000 / dit, 500 / dit * 1.5f, 0));
            Sprite Font = new Sprite(texture, transformFont, colorFont);
            MenuState._renderer.Add(Font);
            typedText.Add(Font);
            TextArray.Add(Font);
        }
    }
}
