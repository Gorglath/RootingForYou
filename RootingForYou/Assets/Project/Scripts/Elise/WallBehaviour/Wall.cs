using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private float speed;

    private bool isTriggered = false;
    public GameObject shadow;
    public DelayedEvent[] onWallMovement; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();

    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;

    }

    public GameObject GetShadow()
    {
        Debug.Log("Shadow: " + shadow.name.ToString());
        return shadow;
    }
    public void UpdatePosition()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        DelayedEventManager.m_instance.InvokeDelayedEvents(onWallMovement);
    }

    void onTriggerPositionReached() { 
        isTriggered = true;
        Debug.Log("Wall is triggered");
    }

    public bool GetIsTriggered()
    {
        return isTriggered;
    }

    public void SetIsTriggered(bool value)
    {
        isTriggered = value;
    }
}
