using UnityEngine;

public class PuzzleWall : PuzzleElement
{
    public override bool IsPushable()
    {
        return false;
    }

    public override bool IsSolid()
    {
        return true;
    }
}
