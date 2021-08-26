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
        public MODE mode;           // ����� ����.
        public float moveSpeed;     // �̵� �ӵ�.
        public float appearRate;    // ���� �ֱ�.
        public float standTime;     // ���ִ� �ð�.
        public int targetHp;        // Ÿ���� ü��.
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
