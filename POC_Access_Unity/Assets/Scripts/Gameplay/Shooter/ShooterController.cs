using UnityEngine;
using UnityEngine.InputSystem;

using UnityUtility.CustomAttributes;

[RequireComponent(typeof(Rigidbody))]
public class ShooterController : MonoBehaviour
{

    [SerializeField] private Transform m_raycastStart;

    [Title("Movement")]
    [SerializeField] private InputActionReference m_moveCameraPositiveAction;
    [SerializeField] private InputActionReference m_moveCameraNegativeAction;
    [SerializeField] private InputActionReference m_shootAction;

    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_cameraMovementSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update()
    {
        float moveCameraInput = m_moveCameraPositiveAction.action.ReadValue<float>() - m_moveCameraNegativeAction.action.ReadValue<float>();
        // Controller Rotation
        float camX = moveCameraInput / Screen.width;

        //bool usingMouse = InputControlPath.TryGetDeviceLayout(m_moveCameraAction.action.bindings[0].effectivePath).ToLower() == "mouse";


        // It is assumed that a mouse is used to move the camera so there is no multiplcation by Time.deltaTime
        transform.rotation *= Quaternion.AngleAxis(camX * m_cameraMovementSpeed /* * (usingMouse ? 1.0f : Time.deltaTime)*/, transform.up);


        if (m_shootAction.action.WasPressedThisFrame())
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        if (Physics.Raycast(m_raycastStart.position, m_raycastStart.forward, out RaycastHit hitInfos))
        {
            if (hitInfos.collider.TryGetComponent(out ShooterEnemy hitEnemy))
            {
                hitEnemy.Kill();
            }
        }
    }
}
