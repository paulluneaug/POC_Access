using UnityUtility.Singletons;

public class GameOptionsManager : MonoBehaviourSingleton<GameOptionsManager>
{
    public bool IsInvincible = false;
    public float GameSpeed = 1;
    public bool IsHighContrast = false;
    public bool IsWindowed = false;
}
