using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private HandGrabbingHelper[] m_groundDetectors = null;
    [SerializeField] private Rigidbody[] m_legsRigidbodies = null;
    [SerializeField] private Rigidbody[] m_bodyRigidbodies = null;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_bodyGravityForce = 10.0f;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_bodyRotationForce = 10.0f;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_bodySnappingUpForce = 10.0f;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_bodySnappingUpDrag = 10.0f;

    //helpers
    private int m_locksCount = 0;
    private bool m_isLocked = false;
    private bool m_isTouchingGround = false;
    public void LockBody()
    {
        m_locksCount++;

        if (m_locksCount > 1)
            return;

        m_isLocked = true;
    }
    public void LockBodyUpRotation()
    {
        foreach (Rigidbody body in m_bodyRigidbodies)
        {
            Vector3 springTorque = m_bodySnappingUpForce * Vector3.Cross(body.transform.right,Vector3.right);
            Vector3 dampTorque = m_bodySnappingUpDrag * -body.angularVelocity;
            body.AddTorque(springTorque + dampTorque, ForceMode.Acceleration);
        }
    }
    private void ApplyGravity()
    {
        foreach (Rigidbody body in m_legsRigidbodies)
        {
            body.AddForce(Vector2.down * m_bodyGravityForce);
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

        m_isTouchingGround = m_groundDetectors[0].GetIsTouchingGround() && m_groundDetectors[1].GetIsTouchingGround();

        if(!m_isTouchingGround)
            ApplyGravity();

        foreach (Rigidbody body in m_bodyRigidbodies)
        {
            Vector3 springTorque = m_bodySnappingUpForce * Vector3.Cross(body.transform.up, Vector3.up);
            Vector3 dampTorque = m_bodySnappingUpDrag * -body.angularVelocity;
            body.AddTorque(springTorque + dampTorque, ForceMode.Acceleration);
        }
    }
}
