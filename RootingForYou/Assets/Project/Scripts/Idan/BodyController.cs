using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Rigidbody m_rootRigidbody = null;
    [SerializeField] private Rigidbody[] m_bodyRigidbodies = null;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_bodyRotationForce = 10.0f;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_bodySnappingUpForce = 10.0f;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_bodySnappingUpDrag = 10.0f;

    //helpers
    private int m_locksCount = 0;
    private bool m_isLocked = false;
    public void LockBody()
    {
        m_locksCount++;

        if (m_locksCount > 1)
            return;

        m_isLocked = true;
    }
    public void LockBodyUpRotation()
    {
        Quaternion rot;
        foreach (Rigidbody body in m_bodyRigidbodies)
        {
            Vector3 springTorque = m_bodySnappingUpForce * Vector3.Cross(body.transform.right,Vector3.right);
            Vector3 dampTorque = m_bodySnappingUpDrag * -body.angularVelocity;
            body.AddTorque(springTorque + dampTorque, ForceMode.Acceleration);
        }
    }
    public void UnlockBody()
    {
        m_locksCount--;

        if (m_locksCount > 0)
            return;

        m_isLocked = false;
    }
    public void ApplyForceToBody(Vector2 Direction)
    {
        foreach (Rigidbody body in m_bodyRigidbodies)
        {
            body.AddForce(Direction * m_bodyRotationForce);
        }
    }

    private void FixedUpdate()
    {
        LockBodyUpRotation();
        if (m_isLocked)
            return;

        Quaternion rot;
        foreach (Rigidbody body in m_bodyRigidbodies)
        {
            Vector3 springTorque = m_bodySnappingUpForce * Vector3.Cross(body.transform.up, Vector3.up);
            Vector3 dampTorque = m_bodySnappingUpDrag * -body.angularVelocity;
            body.AddTorque(springTorque + dampTorque, ForceMode.Acceleration);
        }
    }
}
