using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;



namespace SSJtest;

public static class SimulateInput
{
    public static void ReleaseC()
    {
        var keyboard = Keyboard.current;

        var state = new KeyboardState();
        state.Release(Key.C);

        InputSystem.QueueStateEvent(keyboard, state);

        Plugin.Logger.LogInfo("Released C key");
    }

    public static void PressSpace()
    {
        var keyboard = Keyboard.current;

        var state = new KeyboardState();
        state.Press(Key.Space);

        InputSystem.QueueStateEvent(keyboard, state);

        Plugin.Logger.LogInfo("Pressed Space Key");
    }
}