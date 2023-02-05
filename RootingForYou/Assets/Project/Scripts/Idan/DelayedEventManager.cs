using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedEventManager : MonoBehaviour
{
    public static DelayedEventManager m_instance;

    private void Awake()
    {
        if (!m_instance)
            m_instance = this;
        else
            Destroy(gameObject);
    }
    public void InvokeDelayedEvents(DelayedEvent[] delayedEvents)
    {
        StartCoroutine(EInvokeDelayedEvents(delayedEvents));
    }

    IEnumerator EInvokeDelayedEvents(DelayedEvent[] delayedEvents)
    {
        foreach (DelayedEvent delayedEvent in delayedEvents)
        {
            yield return new WaitForSeconds(delayedEvent.m_timeBeforeInvokingEvent);
            delayedEvent.m_relatedEvent.Invoke();
        }
    }
}
