using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    public bool IsAlive => gameObject.activeSelf;

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
