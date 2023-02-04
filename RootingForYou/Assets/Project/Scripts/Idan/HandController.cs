using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private bool m_isToggleTypeGrab = false;
    [SerializeField] private PlayerInput m_playerInput = null;
    [SerializeField] private string m_moveActionName = null;
    [SerializeField] private string m_lockActionName = null;
    [SerializeField] private BodyController m_bodyController = null;
    [SerializeField] private Rigidbody m_handRigidbody = null;
    [SerializeField] private Rigidbody[] m_armRigidbodies = null;
    [SerializeField] private GameObject m_connectionPoint = null;
    [SerializeField] private HandGrabbingHelper m_handGrabbingHelper = null;
    [Range(1.0f, 70.0f)] [SerializeField] private float m_maxDistanceFromBody = 1.0f;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_forceMultiplier = 10.0f;
    [Range(1.0f, 10.0f)] [SerializeField] private float m_movementDrag = 5.0f;
    //helpers
    private CharacterJoint[] m_connectedPointJoints = null;
    private Vector2 m_inputDirection = Vector2.zero;
    private bool m_enabledPhysics = true;
    private bool m_isLocked = false;
    private void Start()
    {
        m_connectedPointJoints = m_connectionPoint.GetComponents<CharacterJoint>();
    }
    public void SetGrabType(bool isToggle)
    {
        m_isToggleTypeGrab = isToggle;
    }
    private void Update()
    {
        m_inputDirection = m_playerInput.actions[m_moveActionName].ReadValue<Vector2>();

        if (m_playerInput.actions[m_lockActionName].WasReleasedThisFrame())
        {
            if (m_isToggleTypeGrab)
                return;
            UnlockHand();
        }

        if (m_playerInput.actions[m_lockActionName].WasPressedThisFrame())
        {
            if (m_isToggleTypeGrab && m_isLocked)
            {
                UnlockHand();
            }
            else
            {
                LockHand();
            }
            
        }
        
    }
    private void UnfreezeHand()
    {
        foreach (Rigidbody armBody in m_armRigidbodies)
        {
            armBody.isKinematic = false;
        }

        m_handRigidbody.isKinematic = false;
    }
    private void FreezeHand()
    {
        foreach (Rigidbody armBody in m_armRigidbodies)
        {
            armBody.isKinematic = true;
        }

        m_handRigidbody.isKinematic = true;
    }
    private void UnlockHand()
    {
        if (!m_isLocked)
            return;

        m_bodyController.UnlockBody();
        m_isLocked = false;
        m_handGrabbingHelper.SetIsAbleToGrab(true);

        m_connectionPoint.transform.position = m_handRigidbody.position;

        m_connectedPointJoints[0].connectedBody = null;
        m_connectedPointJoints[1].connectedBody = null;
    }
    private void LockHand()
    {
        if (m_isLocked)
            return;

        Rigidbody body = m_handGrabbingHelper.GetAvailableRigidbody();
        if (!body)
            return;

        m_bodyController.LockBody();
        m_isLocked = true;

        m_connectionPoint.transform.position = m_handRigidbody.position;
        
        m_connectedPointJoints[0].connectedBody = body;
        m_connectedPointJoints[1].connectedBody = m_handRigidbody;
        m_handGrabbingHelper.SetIsAbleToGrab(false);

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
        float multiplier = Vector3.Distance(m_handRigidbody.position, m_bodyController.transform.position) / m_maxDistanceFromBody;

        m_bodyController.ApplyForceToBody(m_inputDirection * multiplier);
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

        float multiplier = Vector3.Distance(m_handRigidbody.position, m_bodyController.transform.position) / m_maxDistanceFromBody;
        m_handRigidbody.AddForce(m_inputDirection * m_forceMultiplier * multiplier);

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
}
