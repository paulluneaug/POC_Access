using System;

using UnityEngine;

public class PuzzleBox : PuzzleElement
{
    [SerializeField] private Transform m_inTargetMarker;
    [SerializeField] private Transform m_notInTargetMarker;

    [NonSerialized] private int m_inTargetCount = 0;

    private void Start()
    {
        m_inTargetCount = 0;
    }

    public override bool IsPushable()
    {
        return true;
    }

    public override bool IsSolid()
    {
        return true;
    }

    public void EnterTarget()
    {
        m_inTargetCount++;
        UpdateIsInTarget();
    }

    public void ExitTarget()
    {
        m_inTargetCount--;
        UpdateIsInTarget();
    }

    private void UpdateIsInTarget()
    {
        bool isInTarget = m_inTargetCount > 0;
        m_inTargetMarker.gameObject.SetActive(isInTarget);
        m_notInTargetMarker.gameObject.SetActive(!isInTarget);
    }
}
