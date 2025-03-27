using UnityEngine;

using UnityUtility.ObservableFields;
using UnityUtility.Singletons;

public class GameOptionsManager : MonoBehaviourSingleton<GameOptionsManager>
{
    public bool IsInvincible = false;
    public bool IsHighContrast = false;
    public ObservableField<float> GameSpeed = new ObservableField<float>(1.0f);
    public ObservableField<bool> IsWindowed = new ObservableField<bool>(false);
}
