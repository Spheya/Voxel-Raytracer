using OpenTK;
using OpenTK.Input;

namespace Game.Engine.Input
{
    class ControllerInput
    {
        public static float getForwardInput()
        {
            var joystickState = Joystick.GetState(0);

            return joystickState.GetAxis(0);
        }
    }
}
