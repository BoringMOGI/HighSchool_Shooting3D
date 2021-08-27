using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] Text timeText;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void SwitchTimer(bool isEnable)
    {
        gameObject.SetActive(isEnable);
    }
    public void SetTime(float time)
    {
        int minute = (int)(time / 60);
        int second = (int)(time % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minute, second);
    }

}
