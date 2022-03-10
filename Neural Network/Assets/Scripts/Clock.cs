using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = FormatTime(Time.time);
    }

    string FormatTime(float time)
    {
        int minutes = (int) time / 60 ;
        int seconds = (int) time - 60 * minutes;

        return string. Format("{0:00}:{1:00}", minutes, seconds);
    }
}
