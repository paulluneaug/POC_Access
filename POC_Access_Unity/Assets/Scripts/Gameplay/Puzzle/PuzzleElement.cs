using UnityEngine;

public abstract class PuzzleElement : MonoBehaviour
{
    /// <summary>
    /// Can have multiple elements at the same place
    /// </summary>
    public abstract bool IsSolid();
    /// <summary>
    /// Can be moved
    /// </summary>
    public abstract bool IsPushable();

    public Vector3 Center => transform.position + new Vector3(0.5f, 0.5f, 0.5f);
}
