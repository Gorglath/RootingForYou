using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabbingHelper : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private string m_tagToGrab = null;

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
        m_availableBodyToGrab = other.GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_availableBodyToGrab)
            return;
        
        if (m_availableBodyToGrab.gameObject != other.gameObject)
            return;

        m_isTouchingTheThing = false;
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
