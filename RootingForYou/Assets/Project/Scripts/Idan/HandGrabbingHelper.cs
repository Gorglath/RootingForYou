using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandGrabbingHelper : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private DelayedEvent[] m_onSlamGround = null;
    [SerializeField] private DelayedEvent[] m_onTouchPlayer = null;

    [SerializeField] private string m_tagToGrab = null;
    [SerializeField] private bool m_isBodyRelated = false;
    [Range(1.0f, 20)] [SerializeField] private float m_magnitudeToTriggerEvent = 1.0f;
    //helpers
    private Rigidbody m_availableBodyToGrab = null;
    private bool m_isAbleToGrab = true;
    private bool m_isTouchingTheThing = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!m_isAbleToGrab)
            return;

        if (!other.CompareTag(m_tagToGrab))
            return;

        if (other.transform.root == transform.root)
            return;

        m_isTouchingTheThing = true;

        if (m_isBodyRelated)
        {
            if (GetComponent<Rigidbody>().velocity.magnitude > m_magnitudeToTriggerEvent)
            {
                if (!other.GetComponentInParent<BodyController>())
                {
                    DelayedEventManager.m_instance.InvokeDelayedEvents(m_onSlamGround);
                }
            }
        }
        else
        {
            if (other.GetComponentInParent<BodyController>())
            {
                DelayedEventManager.m_instance.InvokeDelayedEvents(m_onTouchPlayer);
            }
            m_availableBodyToGrab = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_availableBodyToGrab)
            return;

        m_isTouchingTheThing = false;

        if (m_availableBodyToGrab.gameObject != other.gameObject)
            return;

        m_availableBodyToGrab = null;
    }
    public void SetIsAbleToGrab(bool isAbleToGrab)
    {
        m_isAbleToGrab = isAbleToGrab;
    }
    public bool GetIsTouchingGround() { return m_isTouchingTheThing; }
    public Rigidbody GetAvailableRigidbody()
    {
        return m_availableBodyToGrab;
    }
}
