using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MultiplierLevel
{
    public const float Level1 = 1.5f;
    public const float Level2 = 2.5f;
    public const float Level3 = 3.5f;
    public const float Level4 = 4.5f;
}

public class Score : MonoBehaviour
{
    private int totalScore;
    [Header("Score")]
    public float scoreScale = 50.0f;
    public float baseScoreMultiplier = 1f;
    private float scoreMultiplier = 0.0f;
    public float scoreMultiplierScale = 0.5f;
    public float speedModifier = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        scoreMultiplier = baseScoreMultiplier;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseMultiplier()
    {
        scoreMultiplier += scoreMultiplierScale;
    }
    public void ResetMultiplier()
    {
        scoreMultiplier = baseScoreMultiplier;
    }
    public void DeacreaseMultiplier()
    {
        scoreMultiplier += scoreMultiplierScale;
    }
    public void ComputeScore(float averageDistance)
    {
        totalScore += (int)((averageDistance * scoreScale) * scoreMultiplier);
    }

    public float GetScore()
    {
        return totalScore;
    }
    private void SetScoreMultiplierLevel()
    {

    }
}
