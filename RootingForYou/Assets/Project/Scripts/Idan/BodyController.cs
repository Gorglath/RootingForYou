using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BodyController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private UnityEvent m_onUnfreeze = null;
    
    [Space(20.0f)]

    [SerializeField] private HandGrabbingHelper[] m_groundDetectors = null;
    [SerializeField] private Rigidbody[] m_legsRigidbodies = null;
    [SerializeField] private Rigidbody[] m_bodyRigidbodies = null;
    [SerializeField] private Rigidbody[] m_allBodies = null;
    [SerializeField] private Transform m_leftHand = null;
    [SerializeField] private Transform m_rightHand = null;
    [SerializeField] private Transform m_middleBody = null;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_bodyGravityForce = 10.0f;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_bodyRotationForce = 10.0f;
    [Range(1.0f, 10000.0f)] [SerializeField] private float m_bodySnappingUpForce = 10.0f;
    [Range(0.0f, 3.0f)] [SerializeField] private float m_bodySnappingUpDrag = 10.0f;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_bodyFrozenForce = 100.0f;
    [Range(1.0f, 10000.0f)] [SerializeField] private float m_bodyFrozenSnappingForce = 100.0f;
    [Range(0.0f, 3.0f)] [SerializeField] private float m_bodyFrozenSnappingDrag = 100.0f;

    //helpers
    private int m_locksCount = 0;
    private int m_freezeCount = 0;
    private bool m_isLocked = false;
    private bool m_isFrozen = false;
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
    public void ApplyFrozenForceToBody(Vector2 direction)
    {
        foreach (Rigidbody body in m_bodyRigidbodies)
        {
            body.AddForce(direction * m_bodyFrozenForce);
        }
    }
    public void ApplyForceToBody(Vector2 Direction)
    {
        foreach (Rigidbody body in m_bodyRigidbodies)
        {
            body.AddForce(Direction * m_bodyRotationForce);
        }
    }
    public void FreezeBody()
    {
        m_freezeCount++;

        if (m_freezeCount < 2)
            return;

        m_isFrozen = true;
        foreach (Rigidbody body in m_allBodies)
        {
            body.isKinematic = true;
        }
    }

    public void UnfreezeBody()
    {
        m_freezeCount--;

        m_isFrozen = false;
        foreach (Rigidbody body in m_allBodies)
        {
            body.isKinematic = false;
        }
        if (m_freezeCount > 0)
            return;

    }
    private void HandleFrozenAlignment()
    {
        Vector3 rightHandDirection = (m_rightHand.position - m_middleBody.position).normalized;
        rightHandDirection.z = 1;
        Vector3 leftHandDirection = (m_leftHand.position - m_middleBody.position).normalized;
        leftHandDirection.z = 1;
        Vector3 alignmentDirection = -Vector3.Cross(rightHandDirection, leftHandDirection);

        float dot = Vector3.Dot(rightHandDirection,leftHandDirection);

        alignmentDirection.z = 0;
        Debug.Log(dot);
        Debug.DrawLine(m_middleBody.position, m_middleBody.position + rightHandDirection * 10000, Color.blue, Time.fixedDeltaTime);
        Debug.DrawLine(m_middleBody.position, m_middleBody.position + leftHandDirection * 10000, Color.red, Time.fixedDeltaTime);
        Debug.DrawLine(m_middleBody.position, m_middleBody.position + alignmentDirection * 10000, Color.black, Time.fixedDeltaTime);
        foreach (Rigidbody body in m_bodyRigidbodies)
        {
            Vector3 springTorque = m_bodyFrozenSnappingForce * Vector3.Cross(body.transform.up, alignmentDirection);
            Vector3 dampTorque = m_bodyFrozenSnappingDrag * -body.angularVelocity;
            body.AddTorque(springTorque + dampTorque, ForceMode.Acceleration);
        }
    }
    private void FixedUpdate()
    {
        if(m_locksCount > 0)
        {
            HandleFrozenAlignment();
            return;
        }
        if (m_isLocked)
            return;

        LockBodyUpRotation();

        m_isTouchingGround = m_groundDetectors[0].GetIsTouchingGround() && m_groundDetectors[1].GetIsTouchingGround();

        if (!m_isTouchingGround)
            ApplyGravity();


        foreach (Rigidbody body in m_bodyRigidbodies)
        {
            Vector3 springTorque = m_bodySnappingUpForce * Vector3.Cross(body.transform.up, Vector3.up);
            Vector3 dampTorque = m_bodySnappingUpDrag * -body.angularVelocity;
            body.AddTorque(springTorque + dampTorque, ForceMode.Acceleration);
        }
    }
}
