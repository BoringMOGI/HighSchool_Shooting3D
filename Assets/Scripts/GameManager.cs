using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Target")]
    public Transform targetParent;      // 타겟의 부모 오브젝트.
    public float targetAppearRate;      // 적이 등장하는 주기.
    public float targetStandTime;       // 타겟이 서있는 시간.
    public int maxTargetCount;          // 등장하는 타겟의 개수.

    [Header("UI")]
    public StateInfoUI stateInfoUi;     // 상태 UI.
    public CountDownUI countDownUi;     // 카운트 다운 UI.
    public float maxCountDown;          // 총 카운트 다운 시간.
    private float countDown;            // 카운트 다운 시간.

    [Header("Evenets")]
    public UnityEvent<int, int> OnUpdateScore;

    Target[] allTarget;                 // 모든 타겟 오브젝트 배열.
    float nextTargetTime;               // 다음으로 적이 등장하는 시간.

    int targetCount;                    // 등장하는 적의 개수.
    int outTargetCount;                 // 퇴장한 적의 개수.
    int score;                          // 점수.

    bool isPlaying;                     // 게임이 진행중인가?

    void Start()
    {
        // targetParent하위에 있는 자식들 중 Target을 가진 오브젝트를 전부 검색한다.
        allTarget = targetParent.GetComponentsInChildren<Target>();
        foreach(Target target in allTarget)
        {
            target.OnTargetHit += OnHitTarget;
            target.OnTargetOut += OnOutTarget;
        }
               
        OnUpdateScore?.Invoke(0, 0);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            OnStartGame();
        }
    }

    IEnumerator GameUpdate()
    {
        InitGame();

        while (true)
        {
            countDown -= Time.deltaTime;
            countDownUi.SetCountDown(countDown, maxCountDown);
            if (countDown <= 0.0f)
            {
                break;
            }

            yield return null;
        }

        // 게임 시작.
        PlayerController.isStopController = false;
        nextTargetTime = Time.time + targetAppearRate;

        // 적의 등장.
        while (targetCount > 0)
        {
            OnUpdateTarget();
            yield return null;
        }

        // 적이 다 나갔는지 체크.
        while (outTargetCount < maxTargetCount)
            yield return null;

        OnEndGame();
    }

    void InitGame()
    {
        score = 0;
        targetCount = maxTargetCount;
        outTargetCount = 0;

        PlayerController.isStopController = true;
        countDown = maxCountDown;

        OnUpdateScore?.Invoke(targetCount, score);
    }
    void OnStartGame()
    {
        if (isPlaying == true)
            return;

        isPlaying = true;
        StartCoroutine(GameUpdate());
    }
    void OnEndGame()
    {
        isPlaying = false;
        Debug.Log("게임 종료");
    }

    void OnUpdateTarget()
    {
        // 랜덤한 타겟을 등장시킨다.
        if(nextTargetTime <= Time.time && targetCount > 0)
        {
            int targetIndex = Random.Range(0, allTarget.Length);
            Target target = allTarget[targetIndex];
            if(target.IsAppear == false)
            {
                target.OnStand(targetStandTime);        // 적의 등장.
                targetCount -= 1;                        // 적은 남은 개수 감소.
                OnUpdateScore?.Invoke(targetCount, score);
            }

            nextTargetTime = Time.time + targetAppearRate;
        }
    }
    void OnHitTarget()
    {
        score += 1;

        OnUpdateScore?.Invoke(targetCount, score);
    }
    void OnOutTarget()
    {
        outTargetCount += 1;
        Debug.Log("적의 퇴장");
    }
}
