using System;
using System.Collections;

using UnityEngine.UI;

public static class Utils
{
    public static IEnumerator DelayOneFrameCoroutine(Action action)
    {
        yield return null;
        action?.Invoke();
    }
    
    public static Navigation CloneNavigation(Navigation navigation)
    {
        return new Navigation
        {
            selectOnUp = navigation.selectOnUp, 
            selectOnDown = navigation.selectOnDown, 
            selectOnLeft = navigation.selectOnLeft, 
            selectOnRight = navigation.selectOnRight,
            mode = navigation.mode,
            wrapAround = navigation.wrapAround
        };
    }
}
