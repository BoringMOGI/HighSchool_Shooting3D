using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateInfoUI : MonoBehaviour
{
    public Text currnetBulletText;          // 현재 장탄 수.
    public Text remainingBulletText;        // 남은 탄약의 수.

    
    public void SetBulletText(int currnet, int remaining)
    {
        currnetBulletText.text = currnet.ToString();
        remainingBulletText.text = remaining.ToString();
    }
}
