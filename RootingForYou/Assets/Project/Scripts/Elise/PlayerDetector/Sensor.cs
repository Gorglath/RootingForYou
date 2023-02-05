using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Sensor : MonoBehaviour
{

    bool isHit = false;
    bool isCollision = false;
    void Start()
    {

    }

    void Update()
    {
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up / 2);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right / 2);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward / 5);

    }
 
    public float GetClosestHit()
    {
        float distance = 100.0f;
        Vector3[] dirs = { transform.up - transform.position, transform.right - transform.position, - transform.right - transform.position };

        foreach(Vector3 dir in dirs )
        {
            RaycastHit sensorHit;

            isHit = Physics.Raycast(transform.position, transform.position + dir, out sensorHit, Mathf.Infinity);
            Debug.DrawRay(transform.position, transform.position + dir *10000, Color.red, 20.0f);

            if (isHit && !isCollision)
            {
                if (sensorHit.collider.gameObject.CompareTag("Wall"))
                {
                    if (sensorHit.distance <= 0.05f)
                    {
                        Debug.Log("Loser, you suck, no Burrito for you " + name);
                        isCollision = true;
                        break;
                    }
                    else if (sensorHit.distance < distance)
                    {
                        distance = sensorHit.distance;

                    }
                }
                else
                {
                    break;
                }

            }

        }

        return distance;
    }

    public bool GetIsCollision()
    {
        return isCollision;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Wall"))
        {
            Debug.Log("Wall collided with player mesh");
            isCollision = true;
        }
    }
}
