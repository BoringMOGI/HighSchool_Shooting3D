using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameMode gameMode;

    [Header("Target")]
    public Transform targetParent;      // 타겟의 부모 오브젝트.    
    public Transform minMovePosition;   // 최소로 이동할 수 있는 위치.
    public Transform maxMovePssition;   // 최대로 이동할 수 있는 위치.
    public int appearTargetCount;       // 등장하는 적의 개수.
    public int minTargetCount;          // 최소 등장 개수.
    public int maxTargetCount;          // 최대 등장 개수.

    [Header("UI")]
    public StateInfoUI stateInfoUi;     // 상태 UI.
    public CountDownUI countDownUi;     // 카운트 다운 UI.
    public float maxCountDown;          // 총 카운트 다운 시간.
    private float countDown;            // 카운트 다운 시간.

    //[Header("Evenets")]
    //public UnityEvent<int, int> OnUpdateScore;

    Target[] allTarget;                 // 모든 타겟 오브젝트 배열.
    float nextTargetTime;               // 다음으로 적이 등장하는 시간.

    int remainingTarget;                // 남은 적의 개수.
    int outTargetCount;                 // 퇴장한 적의 개수.
    int score;                          // 점수.

    bool isPlaying;                     // 게임이 진행중인가?

    GameMode.TargetInfo targetInfo;     // 적의 정보.

    void Start()
    {
        // targetParent하위에 있는 자식들 중 Target을 가진 오브젝트를 전부 검색한다.
        allTarget = targetParent.GetComponentsInChildren<Target>();
        foreach(Target target in allTarget)
        {
            target.OnTargetHit += OnHitTarget;
            target.OnTargetOut += OnOutTarget;
        }

        stateInfoUi.SetGameModeText(gameMode.gameMode);
        stateInfoUi.SetScoreText(appearTargetCount, 0);        
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
        nextTargetTime = Time.time + targetInfo.appearRate;

        // 적의 등장.
        while (remainingTarget > 0)
        {
            OnUpdateTarget();
            yield return null;
        }

        // 적이 다 나갔는지 체크.
        while (outTargetCount < appearTargetCount)
            yield return null;

        OnEndGame();
    }

    void InitGame()
    {
        score = 0;
        remainingTarget = appearTargetCount;
        outTargetCount = 0;

        PlayerController.isStopController = true;
        countDown = maxCountDown;

        stateInfoUi.SetScoreText(remainingTarget, score);
    }
    public void OnStartGame()
    {
        if (isPlaying == true)
            return;

        isPlaying = true;
        targetInfo = gameMode.GetTargetInfo();
        StartCoroutine(GameUpdate());
    }
    private void OnEndGame()
    {
        isPlaying = false;
        Debug.Log("게임 종료");

        stateInfoUi.SetScoreText(appearTargetCount, score);
    }

    public void OnChangeMode()
    {
        if (isPlaying == true)
            return;

        gameMode.gameMode += 1; // 모드 증가.
        if (gameMode.gameMode == GameMode.MODE.Count)
            gameMode.gameMode = 0;

        stateInfoUi.SetGameModeText(gameMode.gameMode);
    }
    public void OnChangeTargetCount(bool isAdd)
    {
        if (isPlaying == true)
            return;

        appearTargetCount = Mathf.Clamp(appearTargetCount + (isAdd ? 1 : -1), minTargetCount, maxTargetCount);
        stateInfoUi.SetScoreText(appearTargetCount, score);
    }

    void OnUpdateTarget()
    {
        // 랜덤한 타겟을 등장시킨다.
        if(nextTargetTime <= Time.time && remainingTarget > 0)
        {
            int targetIndex = Random.Range(0, allTarget.Length);
            Target target = allTarget[targetIndex];
            if(target.IsAppear == false)
            {
                target.OnStand(targetInfo, minMovePosition.position, maxMovePssition.position);   // 적의 등장.

                remainingTarget -= 1;                                          // 적은 남은 개수 감소.
                stateInfoUi.SetScoreText(remainingTarget, score);
            }

            nextTargetTime = Time.time + targetInfo.appearRate;
        }
    }
    void OnHitTarget()
    {
        score += 1;
        stateInfoUi.SetScoreText(remainingTarget, score);
    }
    void OnOutTarget()
    {
        outTargetCount += 1;
    }
}
