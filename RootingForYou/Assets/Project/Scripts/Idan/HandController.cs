using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [Header("Parameters")]

    [SerializeField] private UnityEvent m_onGrabbing = null;
    [SerializeField] private UnityEvent m_onUngrabbing = null;
    [SerializeField] private UnityEvent m_onFreeze = null;
    [SerializeField] private UnityEvent m_onUnfreeze = null;

    [Space(20.0f)]
    [SerializeField] private bool m_isToggleTypeGrab = false;
    [SerializeField] private PlayerInput m_playerInput = null;
    [SerializeField] private string m_moveActionName = null;
    [SerializeField] private string m_lockActionName = null;
    [SerializeField] private string m_freezeActionName = null;
    [SerializeField] private BodyController m_bodyController = null;
    [SerializeField] private Rigidbody m_handRigidbody = null;
    [SerializeField] private Rigidbody[] m_armRigidbodies = null;
    [SerializeField] private GameObject m_connectionPoint = null;
    [SerializeField] private HandGrabbingHelper m_handGrabbingHelper = null;
    [SerializeField] private ParticleSystem m_grabbingPlayerParticleEffect = null;
    [SerializeField] private ParticleSystem m_ungrabbingPlayerParticleEffect = null;
    [SerializeField] private ParticleSystem m_freezingParticleEffect = null;
    [SerializeField] private ParticleSystem m_unfreezingParticleEffect = null;
   
    [Range(1.0f, 70.0f)] [SerializeField] private float m_maxDistanceFromBody = 1.0f;
    [Range(1.0f, 1000.0f)] [SerializeField] private float m_forceMultiplier = 10.0f;
    [Range(1.0f, 10000.0f)] [SerializeField] private float m_frozenForceMultiplier = 10.0f;
    [Range(1.0f, 10000.0f)] [SerializeField] private float m_frozenForceDrag = 10.0f;
    [Range(1.0f, 10.0f)] [SerializeField] private float m_movementDrag = 5.0f;
    [SerializeField] private bool m_isLeftHanded = false;
    //helpers
    private CharacterJoint[] m_connectedPointJoints = null;
    private BodyController m_connectedBodyController = null;
    private Vector3 m_frozenDirection = Vector3.zero;
    private Vector2 m_inputDirection = Vector2.zero;
    private bool m_enabledPhysics = true;
    private bool m_isLocked = false;
    private bool m_isFrozen = false;
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
        if (m_playerInput.actions[m_freezeActionName].WasPressedThisFrame())
        {
            if (m_isFrozen)
                UnfreezeHand();
            else
                FreezeHand();
        }

        if (m_isFrozen)
            return;

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

    private void HandleFrozenMovement()
    {
        Vector3 springTorque = m_frozenForceMultiplier * Vector3.Cross(m_handRigidbody.transform.right, m_frozenDirection);
        Vector3 dampTorque = m_frozenForceDrag * -m_handRigidbody.angularVelocity;
        m_handRigidbody.AddTorque(springTorque + dampTorque, ForceMode.Acceleration);

        foreach (Rigidbody body in m_armRigidbodies)
        {
            springTorque = m_frozenForceMultiplier * Vector3.Cross(body.transform.right, m_frozenDirection);
            dampTorque = m_frozenForceDrag * -body.angularVelocity;
            body.AddTorque(springTorque + dampTorque, ForceMode.Acceleration);
        }

        if (m_isLocked)
        {
            if(!m_isLeftHanded)
                m_bodyController.ApplyFrozenForceToBody(-m_frozenDirection);
            else
                m_bodyController.ApplyFrozenForceToBody(m_frozenDirection);
        }
    }
    private void UnfreezeHand()
    {
        m_isFrozen = false;

        m_onUnfreeze.Invoke();

        foreach (Rigidbody armBody in m_armRigidbodies)
        {
            armBody.isKinematic = false;
        }

        m_bodyController.UnfreezeBody();

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
    private void FreezeHand()
    {
        m_isFrozen = true;
        m_onFreeze.Invoke();

        if(!m_isLeftHanded)
            m_frozenDirection = (m_handRigidbody.position - m_armRigidbodies[0].position);
        else 
            m_frozenDirection = (m_armRigidbodies[0].position - m_handRigidbody.position);

        m_bodyController.FreezeBody();
    }
    private void UnlockHand()
    {
        if (!m_isLocked)
            return;

        m_onUngrabbing.Invoke();

        Rigidbody body = m_handGrabbingHelper.GetAvailableRigidbody();
        m_connectedBodyController = null;
        
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

        m_onGrabbing.Invoke();

        m_connectedBodyController = body.GetComponentInParent<BodyController>();
       
        m_bodyController.LockBody();
        m_isLocked = true;

        m_connectionPoint.transform.position = m_handRigidbody.position;
        
        m_connectedPointJoints[0].connectedBody = body;
        m_connectedPointJoints[1].connectedBody = m_handRigidbody;
        m_handGrabbingHelper.SetIsAbleToGrab(false);

    }
    private void FixedUpdate()
    {
        if (m_isFrozen)
        {
            HandleFrozenMovement();
            return;
        }
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
        if (!m_connectedBodyController)
            return;

        m_connectedBodyController.ApplyForceToBody(-m_inputDirection * multiplier);
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
