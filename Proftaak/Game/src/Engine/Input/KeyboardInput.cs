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

        public static bool IsTurnLeftDown()
        {
            return currentState.IsKeyDown(Key.Left);
        }

        public static bool IsTurnRightDown()
        {
            return currentState.IsKeyDown(Key.Right);
        }
    }
}
