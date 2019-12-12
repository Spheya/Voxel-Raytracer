using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Game.Engine.Input
{
    class MouseInput
    {
        private static MouseState currentState = Mouse.GetState();
        private static Vector2 mouseDelta = new Vector2(0.0f);
        private static Vector2 absolutePos = new Vector2(0.0f);
        private static ButtonState mouseClick = ButtonState.Released;

        public static void Update()
        {
            MouseState newState = Mouse.GetState();
            mouseDelta = new Vector2(newState.X - currentState.X, newState.Y - currentState.Y);
            currentState = newState;
            mouseClick = newState.LeftButton;
        }

        public static void UpdateAbsolutePos(Vector2 absPos)
        {
            absolutePos = absPos;
        }

        public static Vector2 GetRawMousePos()
        {
            return new Vector2(currentState.X, currentState.Y);
        }

        public static Vector2 GetMousePos()
        {
            return absolutePos;
        }

        public static Vector2 GetMouseDelta()
        {
            return mouseDelta;
        }
        public static ButtonState GetMouseLeftButton()
        {
            return mouseClick;
        }
    }
}
