using UnityEngine;

public class PuzzlePlayer : PuzzleElement
{
    public override bool IsPushable()
    {
        return false;
    }

    public override bool IsSolid()
    {
        return false;
    }
}
