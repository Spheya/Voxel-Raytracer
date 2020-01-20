using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.GameStates;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Game.Engine.Input;

namespace Game.Engine
{
    public sealed class Window : GameWindow
    {
        private ApplicationState _state;
        private float totalTime = 0f;

        public Window(ApplicationState state) : base(
                1280,
                720,
                GraphicsMode.Default,
                "Gamer time",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                4,
                5,
                GraphicsContextFlags.ForwardCompatible
            )
        {
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetInteger(GetPName.MaxFragmentUniformVectors));

            _state = state;

            Title += ": OpenGL " + GL.GetString(StringName.Version);
            VSync = VSyncMode.Off;

            _state.AssignWindow(this);

            _state.OnCreate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CursorVisible = true;

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0,0, Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //Vector2 test = MouseInput.GetMousePos();
            //Console.WriteLine(test);
            base.OnUpdateFrame(e);

            float deltatime = (float)e.Time;
            totalTime += deltatime;

            _state.OnUpdate(deltatime);
            CheckForNewState();

            //TODO: Actually do a fixed update
            while (totalTime > 1f / 30f)
            {
                _state.OnFixedUpdate(1f / 30f);
                CheckForNewState();
                totalTime -= 1f / 30f;
            }

            GLGarbageCollector.Process();
        }

        private void HandleKeyboard()
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
                Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            Title = $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Color4 backColor;
            backColor.A = 1.0f;
            backColor.R = 0.1f;
            backColor.G = 0.1f;
            backColor.B = 0.3f;
            GL.ClearColor(backColor);

            _state.OnDraw((float)e.Time);

            SwapBuffers();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _state.OnDestroy();
            GLGarbageCollector.Process();
        }

        private void CheckForNewState()
        {
            if (_state.IsStateRequested())
            {
                _state.OnDestroy();
                _state = _state.GetRequestedState();
                _state.AssignWindow(this);
                _state.OnCreate();
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            //base.OnMouseMove(e);
            //Console.WriteLine(new Vector2(e.X, e.Y));
            MouseInput.UpdateAbsolutePos(new Vector2(e.X, e.Y));
        }
    }
}
