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
//using System.Windows.Forms;

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
            Comma = 126,
            Space = 51,
            BackSpace = 53,
            Period = 127,
            Right = 48,
            Enter = 49
        }
        Texture texture;
        int o = 0;
        public void FontList()
        {
            foreach (var item in OwnList)
            {
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
        public int posSayString = 0;
        public void AddStringText(string sayString, GameWindow window, int VerPos)
        {
            foreach (char letter in sayString.ToUpper())
            {
                Texture texture222;
                if (Convert.ToString(letter) == " ")
                {
                    Console.WriteLine("ok");
                    string letter2 = "Space";
                    texture222 = new Texture(@"res\Keys\" + letter2 + ".png");
                }
                else if (Convert.ToString(letter) == ".")
                {
                    string letter2 = "Period";
                    texture222 = new Texture(@"res\Keys\" + letter2 + ".png");
                }
                else if (Convert.ToString(letter) == "]")
                {
                    string letter2 = "Right";
                    texture222 = new Texture(@"res\Keys\" + letter2 + ".png");
                }
                else if (Convert.ToString(letter) == ",")
                {
                    string letter2 = "Comma";
                    texture222 = new Texture(@"res\Keys\" + letter2 + ".png");
                }
                else
                {
                    texture222 = new Texture(@"res\Keys\" + letter + ".png");
                }
                AddFont(posSayString - 780, window.Height / 2 - 15 - VerPos, texture222, false);
                posSayString += 25;
            }
        }
        public string textString = "";
        public void AddText()
        {
            foreach (var item in theList)
            {
                bool itemDown = KeyboardInput.IsKeyDown(item);
                if (itemDown == true)
                {
                    Console.WriteLine(item);
                    if (OwnList.Contains((keys) item))
                    {
                        if (item == Key.BackSpace)
                        {
                            if (typedText.Count() > 0)
                            {
                                MenuState._renderer.Remove(typedText.Last());
                                typedText.Remove(typedText.Last());
                                PosText -= 25;
                                break;
                            }
                        }
                        else if (item == Key.Enter)
                        {
                            if (typedText.Count() >=  3)
                            {
                                System.Windows.MessageBox.Show("De naam is te kort.");
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Voltooid!");
                            }
                        }
                        else if (PosText >= 12 * 25)
                        {
                            System.Windows.MessageBox.Show("De naam heeft de maximale lengte bereikt.");
                        }
                        else
                        {
                            string newItem;
                            if (item == Key.Space)
                            {
                                newItem = " ";
                            }
                            else if (item == Key.Comma)
                            {
                                newItem = ",";
                            }
                            else if(item == Key.Period)
                            {
                                newItem = ".";
                            }
                            else
                            {
                                newItem = Convert.ToString(item);
                            }
                            textString += newItem;
                            texture = new Texture(@"res\Keys\" + item + ".png");
                            //Console.WriteLine(@"res\Keys\" + item + ".png");
                            AddFont(PosText-710, 310, texture, true);
                            PosText += 25;
                            break;
                        }
                    }
                }
            }
        }
        public List<Sprite> TextArray = new List<Sprite>();
        void AddFont(int x, int y, Texture texture, bool inList)
        {
            int dit = 40;
            Colour colorFont = new Colour(1, 1, 1);
            Transform transformFont = new Transform(new Vector3(x, y, 0), Vector3.Zero, new Vector3(1000 / dit, 500 / dit * 1.5f, 0));
            Sprite Font = new Sprite(texture, transformFont, colorFont);
            MenuState._renderer.Add(Font);
            TextArray.Add(Font);
            if (inList == true)
            {
                typedText.Add(Font);
            }
        }
    }
}
