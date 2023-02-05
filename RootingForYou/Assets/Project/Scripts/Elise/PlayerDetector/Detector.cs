using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SensorGroups
{
    [SerializeField]
    public string groupName;
    [SerializeField]
    public Sensor[] sensors;

}

public enum State
{
    Success,
    Failed
}

public class Detector : MonoBehaviour
{
    // Start is called before the first frame update
    [ExecuteInEditMode]
    [SerializeField]
    //private Sensor[] sensors;
    [Header("Sensor Groups")]
    private SensorGroups[] groups;

    private float averageDistance = 0.0f;
    public bool isColliding = false;



    private State state = State.Success;
    Sensor[] GetSensors()
    {
        Sensor[] sensors = FindObjectsOfType<Sensor>();

        return sensors;
    }

    void Start()
    {
        //sensors = GetSensors();

    }

    public float GetAverageDistance()
    {
        return averageDistance;
    }

    public State GetState()
    {
        return state;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void CheckPosition()
    {
        float totalDistance = 0.0f;
        int numGroups = 0;
        isColliding = false;

        foreach (SensorGroups group in groups)
        {
            numGroups++;

            float closestDistance = 200.0f;
            bool hasCollided = false;

            foreach (Sensor sensor in group.sensors)
            {
                //Debug.Log(sensor.name.ToString());

                float dist = sensor.GetClosestHit();
                isColliding = sensor.GetIsCollision();

                if (!isColliding)
                {
                    if( dist < closestDistance)
                    {
                        closestDistance = dist;

                    }
                }
                else
                {
                    hasCollided = true;
                    break;
                }
            }

            if (!hasCollided)
            {
                totalDistance += closestDistance;
            }
            else
            {
                break;
            }

        }

        averageDistance = totalDistance / numGroups;
    }

    public void SetState(State newState)
    {
        state = newState;
    }
}
