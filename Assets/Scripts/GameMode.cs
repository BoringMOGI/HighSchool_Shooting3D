using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public enum MODE
    {
        Easy,
        Normal,
        Hard,

        Count,
    }

    [System.Serializable]
    public struct TargetInfo
    {
        public MODE mode;           // 모드의 종류.
        public float moveSpeed;     // 이동 속도.
        public float appearRate;    // 등장 주기.
        public float standTime;     // 서있는 시간.
        public int targetHp;        // 타겟의 체력.
    }

    public MODE gameMode;
    public TargetInfo[] targetInfos;

    public TargetInfo GetTargetInfo()
    {
        TargetInfo info = new TargetInfo();
        for(int i = 0; i<targetInfos.Length; i++)
        {
            if(targetInfos[i].mode == gameMode)
            {
                info = targetInfos[i];
                break;
            }
        }

        return info;
    }
}
