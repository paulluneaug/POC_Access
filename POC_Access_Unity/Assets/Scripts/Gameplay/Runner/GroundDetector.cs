using System;
using UnityEngine;
using UnityUtility.Extensions;

public class GroundDetector : MonoBehaviour
{
    public bool Grounded => m_groundObjectsTouching > 0;

    [SerializeField] private TriggerObject m_trigger;
    [SerializeField] private LayerMask m_groundLayers;

    [NonSerialized] private int m_groundObjectsTouching;

    private void Awake()
    {
        m_groundObjectsTouching = 0;
        m_trigger.OnEnter += OnEnterTrigger;
        m_trigger.OnExit += OnExitTrigger;
    }

    private void OnDestroy()
    {
        m_trigger.OnEnter -= OnEnterTrigger;
        m_trigger.OnExit -= OnExitTrigger;
    }

    private void OnEnterTrigger(Collider collider)
    {
        if (m_groundLayers.Contains(collider.gameObject.layer))
        {
            m_groundObjectsTouching++;
        }
    }

    private void OnExitTrigger(Collider collider)
    {
        if (m_groundLayers.Contains(collider.gameObject.layer))
        {
            m_groundObjectsTouching--;
        }
    }
}
