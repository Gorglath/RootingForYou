using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    private int score;
    private float timer;
    private int multiplier;

    public TMP_Text gameOverScoreText;
    public TMP_Text hudScoreText;
    public TMP_Text timerText;

    public Slider slider;

    public int getScore() { return score; }
    public float getTimer() { return timer; }
    public int getMultiplier() { return multiplier; }

    private void Start()
    {
        initMuliptlier();
        score = 0;
        timer = 60;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            setScore(score + 10);
        updateMultiplier();
        updateScoreText();
        updateTimer();
    }

    public void setScore(int pScore)
    {
        score = pScore;
        updateScoreText();
    }

    public void setTimer(int pTimer)
    {
        timer = pTimer;
    }

    public void setMultiplier(int pMultiplier)
    {
        multiplier = pMultiplier;
    }

    public void updateScoreText()
    {
        gameOverScoreText.text = score.ToString();
        hudScoreText.text = score.ToString();
    }

    public void initMuliptlier()
    {
        slider.maxValue = 10;
        slider.value = 0;
    }

    public void updateMultiplier()
    {
        slider.value = multiplier;
    }

    public void updateTimer()
    {
        timerText.text = timer.ToString();
    }
}
