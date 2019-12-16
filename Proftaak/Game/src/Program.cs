using System;
using System.Net;
using System.Text;
using System.Threading;
using Networking;
using Game.Engine;
using Game.GameStates;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Window(new SplashScreenState()).Run();
            new Window(new GameState()).Run();
        }
    }
}
