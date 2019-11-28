using OpenTK;
using OpenTK.Input;

namespace Game.Engine.Input
{
    class KeyboardInput
    {
        public static bool isForwardDown()
        {
            return Keyboard.GetState().IsKeyDown(Key.W);
        }

        public static bool isBackwardDown()
        {
            return Keyboard.GetState().IsKeyDown(Key.S);
        }

        public static bool isStrafeLeftDown()
        {
            return Keyboard.GetState().IsKeyDown(Key.A);
        }

        public static bool isStrafeRightDown()
        {
            return Keyboard.GetState().IsKeyDown(Key.D);
        }
    }
}
