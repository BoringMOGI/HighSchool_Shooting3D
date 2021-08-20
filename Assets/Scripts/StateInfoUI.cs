using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateInfoUI : MonoBehaviour
{
    [Header("PlayerInfo")]
    public Image hpImage;                   // 체력 이미지.
    public Text currnetBulletText;          // 현재 장탄 수.
    public Text remainingBulletText;        // 남은 탄약의 수.

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
