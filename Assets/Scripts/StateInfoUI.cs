using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateInfoUI : MonoBehaviour
{
    [Header("PlayerInfo")]
    public Image hpImage;                   // ü�� �̹���.
    public Text currnetBulletText;          // ���� ��ź ��.
    public Text remainingBulletText;        // ���� ź���� ��.

    [Header("Score")]
    public Text enemyText;
    public Text scoreText;

    public void SetHpImage(float current, float max)
    {
        hpImage.fillAmount = current / max;
    }
    public void SetBulletText(int currnet, int remaining)
    {
        currnetBulletText.text = currnet.ToString();
        remainingBulletText.text = remaining.ToString();
    }

    public void SetScoreText(int enemyCount, int score)
    {
        enemyText.text = enemyCount.ToString();
        scoreText.text = score.ToString();
    }
}
