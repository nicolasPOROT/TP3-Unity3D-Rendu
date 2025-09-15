using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private float timer;
    public float cycleSpeed;
    private Light soleil;

    private void Start()
    {
        soleil = GetComponent<Light>();
    }

    private void Update()
    {
        timer += cycleSpeed * Time.deltaTime;
        //reset day
        if (timer >= 360)
        {
            timer = 0;
        }
        transform.rotation = Quaternion.Euler(timer, 0, 0);

        //set intensities
        if (timer >= 0 && timer <= 90)
        {
            soleil.intensity = Mathf.Lerp(0.3f, 1f, timer / 90);
        }
        else if (timer >= 90 && timer <= 180)
        {
            soleil.intensity = Mathf.Lerp(1f, 0.3f, (timer - 90) / 90);
        }
        else
        {
            soleil.intensity = 0.3f;
        }
    }
}