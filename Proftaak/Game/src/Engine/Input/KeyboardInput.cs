using OpenTK;
using OpenTK.Input;
using System;

namespace Game.Engine.Input
{
    class KeyboardInput
    {
        private static KeyboardState currentState;

        public static void Update()
        {
            currentState = Keyboard.GetState();
        }
        public static bool UpdateReturn()
        {
            return currentState.IsAnyKeyDown;
        }

        public static bool IsKeyDown(Key key)
        {
            return currentState.IsKeyDown(key);
        }

        public static bool IsUpDown()
        {
            return currentState.IsKeyDown(Key.Space);
        }

        public static bool IsDownDown()
        {
            return currentState.IsKeyDown(Key.ShiftLeft);
        }

        public static bool IsForwardDown()
        {
            return currentState.IsKeyDown(Key.W);
        }

        public static bool IsBackwardDown()
        {
            return currentState.IsKeyDown(Key.S);
        }

        public static bool IsStrafeLeftDown()
        {
            return currentState.IsKeyDown(Key.A);
        }

        public static bool IsStrafeRightDown()
        {
            return currentState.IsKeyDown(Key.D);
        }
        public static bool IsMDown()
        {
            return currentState.IsKeyDown(Key.M);
        }
        public static bool IsIDown()
        {
            return currentState.IsKeyDown(Key.I);
        }
        public static bool IsKDown()
        {
            return currentState.IsKeyDown(Key.K);
        }
        public static bool IsEDown()
        {
            return currentState.IsKeyDown(Key.E);
        }
        public static bool IsBackspaceDown()
        {
            return currentState.IsKeyDown(Key.BackSpace);
        }
        public static bool IsAnyDown()
        {
            return currentState.IsAnyKeyDown;
        }
    }
}
