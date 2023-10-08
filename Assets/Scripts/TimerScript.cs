using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public static float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }
}
