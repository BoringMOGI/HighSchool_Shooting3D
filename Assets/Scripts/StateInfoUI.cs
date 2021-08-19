using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateInfoUI : MonoBehaviour
{
    public Text currnetBulletText;          // ���� ��ź ��.
    public Text remainingBulletText;        // ���� ź���� ��.

    
    public void SetBulletText(int currnet, int remaining)
    {
        currnetBulletText.text = currnet.ToString();
        remainingBulletText.text = remaining.ToString();
    }
}
