using UnityEngine;

using UnityUtility.Extensions;

public class PuzzlePlayer : PuzzleElement
{
    [SerializeField] private Transform m_model;
    public override bool IsPushable()
    {
        return false;
    }

    public override bool IsSolid()
    {
        return false;
    }

    public override void Move(Vector2 offset)
    {
        base.Move(offset);
        m_model.rotation = Quaternion.LookRotation(offset.X0Y(), Vector3.up);
    }
}
