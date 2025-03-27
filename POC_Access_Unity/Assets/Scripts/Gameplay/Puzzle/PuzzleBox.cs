using System;

using UnityEngine;

public class PuzzleBox : PuzzleElement
{
    [SerializeField] private Transform m_inTargetMarker;
    [SerializeField] private Transform m_notInTargetMarker;

    [SerializeField] private AudioSource m_boxMoveAudio;

    [NonSerialized] private int m_inTargetCount = 0;

    private void Start()
    {
        m_inTargetCount = 0;
        UpdateIsInTarget();
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

    public override void Move(Vector2 offset)
    {
        base.Move(offset);
        m_boxMoveAudio.Play();
    }
}
