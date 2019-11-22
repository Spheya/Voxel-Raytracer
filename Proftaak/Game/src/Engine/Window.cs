using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.GameStates;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Game.Engine
{
    public sealed class Window : GameWindow
    {
        private ApplicationState _state;

        public Window(ApplicationState state) : base(
                1280,
                720,
                GraphicsMode.Default,
                "Gamer time",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                4,
                0,
                GraphicsContextFlags.ForwardCompatible
            )
        {
            _state = state;

            Title += ": OpenGL " + GL.GetString(StringName.Version);
            VSync = VSyncMode.Off;
        }

        protected override void OnLoad(EventArgs e)
        {
            CursorVisible = true;
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0,0, Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _state.OnUpdate((float)e.Time);
            CheckForNewState();
            
            //TODO: Actually do a fixed update
            _state.OnFixedUpdate((float)e.Time);
            CheckForNewState();
        }

        private void HandleKeyboard()
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
                Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //Title = $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";

            Color4 backColor;
            backColor.A = 1.0f;
            backColor.R = 0.1f;
            backColor.G = 0.1f;
            backColor.B = 0.3f;
            GL.ClearColor(backColor);
            
            _state.OnDraw((float)e.Time);

            SwapBuffers();
        }

        private void CheckForNewState()
        {
            if (_state.IsStateRequested())
            {
                _state.OnDestroy();
                _state = _state.GetRequestedState();
                _state.OnCreate();
            }
        }
    }
}
