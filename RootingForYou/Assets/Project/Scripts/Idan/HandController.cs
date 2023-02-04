using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private PlayerInput m_playerInput = null;
    [SerializeField] private string m_moveActionName = null;
    [SerializeField] private string m_lockActionName = null;
    [SerializeField] private BodyController m_bodyController = null;
    [SerializeField] private Rigidbody m_handRigidbody = null;
    [SerializeField] private Rigidbody[] m_armRigidbodies = null;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_forceMultiplier = 10.0f;
    [Range(1.0f, 10.0f)] [SerializeField] private float m_movementDrag = 5.0f;
    //helpers
    private GameObject m_grabbableObject = null;
    //private Vector3 m_grabLocation = Vector3.zero;
    private Vector2 m_inputDirection = Vector2.zero;
    private bool m_enabledPhysics = true;
    private bool m_isAbleToGrab = false;
    private bool m_isLocked = false;
    private void Update()
    {
        m_inputDirection = m_playerInput.actions[m_moveActionName].ReadValue<Vector2>();

        if (m_playerInput.actions[m_lockActionName].WasReleasedThisFrame())
        {
            UnlockHand();
        }

        if (!m_isAbleToGrab)
            return;

        if (m_playerInput.actions[m_lockActionName].WasPressedThisFrame())
        {
            LockHand();
        }
        
    }

    private void UnlockHand()
    {
        if (!m_isLocked)
            return;

        m_bodyController.UnlockBody();
        m_handRigidbody.isKinematic = false;
        m_isLocked = false;
    }
    private void LockHand()
    {
        if (m_isLocked)
            return;

        m_bodyController.LockBody();
        m_handRigidbody.isKinematic = true;
        m_isLocked = true;
    }
    private void FixedUpdate()
    {
        if (!m_isLocked)
        {
            HandleHandMovement();
        }
        else
        {
            HandleBodyMovement();
        }
    }

    private void HandleBodyMovement()
    {
        if (m_inputDirection.magnitude == 0)
            return;

        Vector3 Direction = Vector3.Cross(-m_inputDirection, Vector3.up);
        m_bodyController.ApplyForceToBody(m_inputDirection);
    }
    private void HandleHandMovement()
    {
        if (m_inputDirection.magnitude == 0)
        {
            if (m_enabledPhysics)
                return;

            m_enabledPhysics = true;

            foreach (Rigidbody armBody in m_armRigidbodies)
            {
                armBody.useGravity = true;
            }

            m_handRigidbody.useGravity = true;
            m_handRigidbody.drag = 0.0f;
            return;
        }

        m_handRigidbody.AddForce(m_inputDirection * m_forceMultiplier);

        if (!m_enabledPhysics)
            return;

        m_enabledPhysics = false;

        foreach (Rigidbody armBody in m_armRigidbodies)
        {
            armBody.useGravity = false;
        }

        m_handRigidbody.useGravity = false;
        m_handRigidbody.drag = m_movementDrag;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Grabbable"))
            return;

        m_grabbableObject = collision.gameObject;
        //m_grabLocation = collision.GetContact(0).point;
        m_isAbleToGrab = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!m_grabbableObject)
            return;

        if (m_grabbableObject != collision.gameObject)
            return;

        m_grabbableObject = null;
        m_isAbleToGrab = false;
    }
}
