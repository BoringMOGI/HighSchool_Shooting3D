using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUI : MonoBehaviour
{
    public Image blindImage;
    public Text countText;

    public void SetCountDown(float currentTime, float maxTime)
    {
        blindImage.enabled = true;
        countText.text = ((int)currentTime + 1).ToString();
        if(currentTime <= 0.0f)
        {
            blindImage.enabled = false;
            countText.text = string.Empty;
        }
    }
}
