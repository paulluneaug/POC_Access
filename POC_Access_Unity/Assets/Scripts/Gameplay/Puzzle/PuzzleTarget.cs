using System;
using UnityEngine;
using UnityUtility.Extensions;

public class PuzzleTarget : PuzzleElement
{
    [NonSerialized] private bool m_hasBox;

    public override bool IsPushable()
    {
        return false;
    }

    public override bool IsSolid()
    {
        return false;
    }

    public bool HasBox()
    {
        return m_hasBox;
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.HasComponent<PuzzleBox>())
        {
            m_hasBox = true;
            Debug.Log($"{name} has {other.name}");
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.HasComponent<PuzzleBox>())
        {
            m_hasBox = false;
            Debug.Log($"{name} no longer has {other.name}");
        }
    }
}
