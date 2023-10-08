using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardTimes : MonoBehaviour
{
    public Text timeText;  // Assign your Text component in the inspector

    void Start()
    {
        float elapsedTime = PlayerPrefs.GetFloat("ElapsedTime");
        timeText.text = elapsedTime.ToString("F2");  // Display time with 2 decimal places
    }
}
