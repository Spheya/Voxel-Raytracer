using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Game.GameStates
{
    sealed class Game : GameState
    {
        int shader;

        public override void OnCreate()
        {
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, File.ReadAllText(@"res\fragment.glsl"));
            GL.CompileShader(fragmentShader);

            shader = GL.CreateProgram();
            GL.AttachShader(shader, fragmentShader);
            GL.LinkProgram(shader);

            GL.DetachShader(shader, fragmentShader);
            GL.DeleteShader(fragmentShader);
        }

        public override void OnUpdate(float deltatime)
        {
        }

        public override void OnFixedUpdate(float deltatime)
        {
        }

        public override void OnDraw(float deltatime)
        {
        }

        public override void OnDestroy()
        {
        }
    }
}
