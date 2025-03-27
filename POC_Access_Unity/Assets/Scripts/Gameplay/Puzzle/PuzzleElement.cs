using UnityEngine;

using UnityUtility.Extensions;

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

    public virtual void Move(Vector2 offset)
    {
        transform.position += offset.X0Y();
    }
}
