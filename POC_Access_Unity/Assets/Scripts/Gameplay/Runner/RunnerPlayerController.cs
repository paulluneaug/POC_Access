using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtility.CustomAttributes;

public class RunnerPlayerController : MonoBehaviour
{
    [Title("Controller Settings")]
    [SerializeField] private float m_speed;
    [SerializeField] private float m_jumpForce;

    [Title("Inputs")]
    [SerializeField] private InputActionReference m_jumpAction;

    [Title("Components")]
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private BoxCollider m_collider;
    [SerializeField] private GroundDetector m_groundDetector;
    [SerializeField] private Animator m_playerAnimator;
 
    [Title("Misc")]
    [SerializeField] private LayerMask m_groundLayers;
    [SerializeField] private float m_raycastDistance = 0.1f;

    [NonSerialized] private bool m_started = false;

    public void StartPlayer()
    {
        m_started = true;

        m_rigidbody.isKinematic = false;
        m_jumpAction.action.Enable();
        m_jumpAction.action.performed += OnJumpPerformed;
    }

    public void StopPlayer()
    {
        m_started = false;
        m_rigidbody.isKinematic = true;
        m_jumpAction.action.performed -= OnJumpPerformed;
    }

    private void FixedUpdate()
    {
        if (!m_started)
        {
            return;
        }
        bool wallForward = Physics.BoxCast(m_collider.transform.position + m_collider.center, m_collider.size / 2, Vector3.forward, m_collider.transform.rotation, m_raycastDistance, m_groundLayers);
        m_rigidbody.linearVelocity = m_rigidbody.linearVelocity.WhereZ(wallForward ? 0.0f : m_speed);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (m_groundDetector.Grounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        m_playerAnimator.SetTrigger("Jump");
    }
}
