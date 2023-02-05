using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;


public class Player
{
    public bool isValid = true;
    public float averageDistance = 0.0f;
}
public class Master : MonoBehaviour
{
    private WallManager wallManager;
    //public Wall wall;
    public Detector[] detectors;
    private Score scoreManager;
    private Life lifeManager;
    private bool isFirstDeath = true;
    private int GUILives;

    // Start is called before the first frame update
    void Start()
    {


        //GetDetectors();
        wallManager = gameObject.GetComponent<WallManager>();
        Debug.Log(wallManager.GetCurrentWall().gameObject.name.ToString());
        scoreManager = gameObject.GetComponent<Score>();
        lifeManager = gameObject.GetComponent<Life>();
        GUILives = lifeManager.GetNumLives();
    }

    void Update()
    {

        if (wallManager.GetCurrentWall().GetIsTriggered())
        {
            float averageDistance = 0.0f;

            foreach (Detector detector in detectors)
            {
                detector.CheckPosition();

                if (!detector.isColliding)
                {
                    averageDistance += detector.GetAverageDistance();
                    scoreManager.IncreaseMultiplier();
                    wallManager.speed += scoreManager.speedModifier;

                }
                else
                {

                    if (!isFirstDeath)
                    {
                        lifeManager.RemoveLife();
                        wallManager.speed -= scoreManager.speedModifier;
                    }
                    scoreManager.ResetMultiplier();
                    isFirstDeath = false;
                    averageDistance = 0.0f;
                    break;
                }
            }

            scoreManager.ComputeScore(averageDistance);
            wallManager.ChangeWall();
            wallManager.GetCurrentWall().SetIsTriggered(false);


        }


    }
    private void GetDetectors()
    {
        detectors = FindObjectsOfType<Detector>();
    }

    void OnGUI()
    {

        GUI.Label(new Rect(0,0,100,100), GUILives.ToString());
    }

}