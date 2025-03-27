using UnityEngine;

using UnityUtility.Singletons;

public class GameOptionsManager : MonoBehaviourSingleton<GameOptionsManager>
{
    public bool IsInvincible = false;
    public bool IsHighContrast = false;
    public float GameSpeed = 1;
    public bool IsWindowed = false;
}
