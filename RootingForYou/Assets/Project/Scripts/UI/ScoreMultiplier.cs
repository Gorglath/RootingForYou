using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMultiplier : MonoBehaviour
{
    public Slider slider;

    public void SetMultiplier(int pMultiplier)
    {
        slider.value = pMultiplier;
    }

    public void SetMaxMultiplier(int pMaxMultiplier)
    {
        slider.maxValue = pMaxMultiplier;
        slider.value = 0;
    }
}
