using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public enum BUTTON_TYPE    
    { 
        Start,                  // 게임 시작.
        ChangeMode,             // 모드 변경.
        AddEnemyCount,          // 적의 개수 증가.
        DelEnemyCount,          // 적의 개수 감소.
    }

    public GameManager gameManager;


    public void OnButton(BUTTON_TYPE type)
    {
        switch(type)
        {
            case BUTTON_TYPE.Start:
                gameManager.OnStartGame();
                break;

            case BUTTON_TYPE.ChangeMode:
                gameManager.OnChangeMode();
                break;

            case BUTTON_TYPE.AddEnemyCount:
                gameManager.OnChangeTargetCount(true);
                break;

            case BUTTON_TYPE.DelEnemyCount:
                gameManager.OnChangeTargetCount(false);
                break;
        }
    }
}
