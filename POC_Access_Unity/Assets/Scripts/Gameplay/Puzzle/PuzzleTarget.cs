using System;

using UnityEngine;

public class PuzzleTarget : PuzzleElement
{
    [SerializeField] private AudioSource m_targetValidateAudio;
    [SerializeField] private AudioSource m_targetInvalidateMoveAudio;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PuzzleBox box))
        {
            m_hasBox = true;
            Debug.Log($"{name} has {other.name}");
            box.EnterTarget();
            m_targetValidateAudio.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PuzzleBox box))
        {
            m_hasBox = false;
            Debug.Log($"{name} no longer has {other.name}");
            box.ExitTarget();
            m_targetInvalidateMoveAudio.Play();
        }
    }
}
