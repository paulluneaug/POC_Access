using UnityEngine.InputSystem;

public static class InputActionExtensions
{
    public static void SetEnabled(this InputAction action, bool enabled)
    {
        if (enabled)
        {
            action.Enable();
        }
        else
        {
            action.Disable();
        }
    }
}
