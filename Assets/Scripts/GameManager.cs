using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameMode gameMode;

    [Header("Target")]
    public Transform targetParent;      // Ÿ���� �θ� ������Ʈ.    
    public Transform minMovePosition;   // �ּҷ� �̵��� �� �ִ� ��ġ.
    public Transform maxMovePssition;   // �ִ�� �̵��� �� �ִ� ��ġ.
    public int appearTargetCount;       // �����ϴ� ���� ����.
    public int minTargetCount;          // �ּ� ���� ����.
    public int maxTargetCount;          // �ִ� ���� ����.

    [Header("UI")]
    public StateInfoUI stateInfoUi;     // ���� UI.
    public CountDownUI countDownUi;     // ī��Ʈ �ٿ� UI.
    public float maxCountDown;          // �� ī��Ʈ �ٿ� �ð�.
    private float countDown;            // ī��Ʈ �ٿ� �ð�.

    //[Header("Evenets")]
    //public UnityEvent<int, int> OnUpdateScore;

    Target[] allTarget;                 // ��� Ÿ�� ������Ʈ �迭.
    float nextTargetTime;               // �������� ���� �����ϴ� �ð�.

    int remainingTarget;                // ���� ���� ����.
    int outTargetCount;                 // ������ ���� ����.
    int score;                          // ����.

    bool isPlaying;                     // ������ �������ΰ�?

    GameMode.TargetInfo targetInfo;     // ���� ����.

    void Start()
    {
        // targetParent������ �ִ� �ڽĵ� �� Target�� ���� ������Ʈ�� ���� �˻��Ѵ�.
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

        // ���� ����.
        PlayerController.isStopController = false;
        nextTargetTime = Time.time + targetInfo.appearRate;

        // ���� ����.
        while (remainingTarget > 0)
        {
            OnUpdateTarget();
            yield return null;
        }

        // ���� �� �������� üũ.
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
        Debug.Log("���� ����");

        stateInfoUi.SetScoreText(appearTargetCount, score);
    }

    public void OnChangeMode()
    {
        if (isPlaying == true)
            return;

        gameMode.gameMode += 1; // ��� ����.
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
        // ������ Ÿ���� �����Ų��.
        if(nextTargetTime <= Time.time && remainingTarget > 0)
        {
            int targetIndex = Random.Range(0, allTarget.Length);
            Target target = allTarget[targetIndex];
            if(target.IsAppear == false)
            {
                target.OnStand(targetInfo, minMovePosition.position, maxMovePssition.position);   // ���� ����.

                remainingTarget -= 1;                                          // ���� ���� ���� ����.
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
