using OpenTK;
using OpenTK.Input;

namespace Game.Engine.Input
{
    class KeyboardInput
    {
        private static KeyboardState currentState;

        public static void Update()
        {
            currentState = Keyboard.GetState();
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
    }
}
