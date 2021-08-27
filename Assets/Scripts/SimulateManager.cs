using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateManager : MonoBehaviour
{
    [SerializeField] StateInfoUI stateInfoUI;
    [SerializeField] TimerUI timerUI;

    int allTargetCount;
    int score;

    bool isSimulate;
    float flowTime;     // 경과 시간.
    
    void Start()
    {
        isSimulate = true;
        flowTime = 0f;

        TriggetTarget[] allTargets = FindObjectsOfType<TriggetTarget>();
        allTargetCount = allTargets.Length;
        score = 0;

        stateInfoUI.SetScoreText(allTargetCount, score);
    }

    void Update()
    {
        if (isSimulate)
        {
            flowTime += Time.deltaTime;
            timerUI.SetTime(flowTime);
            timerUI.SwitchTimer(true);
        }
    }

    public void OnTargetOut()
    {
        allTargetCount -= 1;
        score += 13;
        stateInfoUI.SetScoreText(allTargetCount, score);
    }
    public void OnEndSimulate()
    {
        if(allTargetCount <= 0)
        {
            isSimulate = false;
        }                
    }
}
