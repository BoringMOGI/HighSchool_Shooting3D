using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Target")]
    public Transform targetParent;      // 타겟의 부모 오브젝트.
    public float targetAppearRate;      // 적이 등장하는 주기.
    public float targetStandTime;       // 타겟이 서있는 시간.

    [Header("Count down")]
    public CountDownUI countDownUi;     // 카운트 다운 UI.
    public float maxCountDown;          // 총 카운트 다운 시간.
    private float countDown;            // 카운트 다운 시간.
    
    Target[] allTarget;                 // 모든 타겟 오브젝트 배열.
    float nextTargetTime;               // 다음으로 적이 등장하는 시간.

    // Start is called before the first frame update
    void Start()
    {
        countDown = maxCountDown;
        PlayerController.isStopController = true;

        // targetParent하위에 있는 자식들 중 Target을 가진 오브젝트를 전부 검색한다.
        allTarget = targetParent.GetComponentsInChildren<Target>();     
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown > 0.0f)
        {
            countDown -= Time.deltaTime;
            countDownUi.SetCountDown(countDown, maxCountDown);
            if (countDown <= 0.0f)
            {
                OnStartGame();
            }

            return;
        }

        // 게임 시작.
        OnUpdateTarget();
    }

    void OnStartGame()
    {
        PlayerController.isStopController = false;
        nextTargetTime = Time.time + targetAppearRate;
    }
    void OnUpdateTarget()
    {
        // 랜덤한 타겟을 등장시킨다.
        if(nextTargetTime <= Time.time)
        {
            int targetIndex = Random.Range(0, allTarget.Length);
            Target target = allTarget[targetIndex];
            if(target.IsAppear == false)
            {
                target.OnStand(targetStandTime);
            }

            nextTargetTime = Time.time + targetAppearRate;
        }
    }
}
