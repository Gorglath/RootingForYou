using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DelayedEvent
{
    public float m_timeBeforeInvokingEvent = 0.0f;
    public UnityEvent m_relatedEvent = null;
}
