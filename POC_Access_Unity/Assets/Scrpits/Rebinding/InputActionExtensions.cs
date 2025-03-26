using System;

using Unity.VisualScripting;

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

    public static bool TryGetBindingForGroup(this InputAction action, string bindingGroup, out int bindingIndex, out InputBinding binding)
    {
        InputBinding bindingMask = InputBinding.MaskByGroup(bindingGroup);
        bindingIndex = action.GetBindingIndex(bindingMask);

        if (bindingIndex != -1)
        {
            binding = action.bindings[bindingIndex];
            return true;
        }
        binding = default;
        return false;
    }
}
