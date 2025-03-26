using UnityEngine;

public class PuzzleBox : PuzzleElement
{
    public override bool IsPushable()
    {
        return true;
    }

    public override bool IsSolid()
    {
        return true;
    }
}
