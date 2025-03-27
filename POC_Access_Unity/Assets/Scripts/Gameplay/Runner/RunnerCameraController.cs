using UnityEngine;
using UnityUtility.MathU;

public class RunnerCameraController : MonoBehaviour
{
    [SerializeField] private Transform m_cameraTarget;
    [SerializeField] private Transform m_cameraLookAtTarget;
    [SerializeField] private float m_cameraFollowHalfLife;
    [SerializeField] private float m_cameraSlerpArgument = 0.5f;



    private void FixedUpdate()
    {
        float camPositionZ = MathUf.SmoothLerp(transform.position.z, m_cameraTarget.position.z, Time.fixedDeltaTime, m_cameraFollowHalfLife);
        Vector3 camPosition = transform.position.WhereZ(camPositionZ);


        Quaternion targetRotation = Quaternion.LookRotation(m_cameraLookAtTarget.position - camPosition, Vector3.up);
        Quaternion camRotation = Quaternion.Slerp(transform.rotation, targetRotation, m_cameraSlerpArgument);

        transform.SetPositionAndRotation(camPosition, camRotation);
    }
}
