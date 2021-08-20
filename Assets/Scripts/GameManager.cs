using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Target")]
    public Transform targetParent;      // Ÿ���� �θ� ������Ʈ.
    public float targetAppearRate;      // ���� �����ϴ� �ֱ�.
    public float targetStandTime;       // Ÿ���� ���ִ� �ð�.
    public int maxTargetCount;          // �����ϴ� Ÿ���� ����.

    [Header("UI")]
    public StateInfoUI stateInfoUi;     // ���� UI.
    public CountDownUI countDownUi;     // ī��Ʈ �ٿ� UI.
    public float maxCountDown;          // �� ī��Ʈ �ٿ� �ð�.
    private float countDown;            // ī��Ʈ �ٿ� �ð�.

    [Header("Evenets")]
    public UnityEvent<int, int> OnUpdateScore;

    Target[] allTarget;                 // ��� Ÿ�� ������Ʈ �迭.
    float nextTargetTime;               // �������� ���� �����ϴ� �ð�.

    int targetCount;                    // �����ϴ� ���� ����.
    int outTargetCount;                 // ������ ���� ����.
    int score;                          // ����.

    bool isPlaying;                     // ������ �������ΰ�?

    void Start()
    {
        // targetParent������ �ִ� �ڽĵ� �� Target�� ���� ������Ʈ�� ���� �˻��Ѵ�.
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

        // ���� ����.
        PlayerController.isStopController = false;
        nextTargetTime = Time.time + targetAppearRate;

        // ���� ����.
        while (targetCount > 0)
        {
            OnUpdateTarget();
            yield return null;
        }

        // ���� �� �������� üũ.
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
        Debug.Log("���� ����");
    }

    void OnUpdateTarget()
    {
        // ������ Ÿ���� �����Ų��.
        if(nextTargetTime <= Time.time && targetCount > 0)
        {
            int targetIndex = Random.Range(0, allTarget.Length);
            Target target = allTarget[targetIndex];
            if(target.IsAppear == false)
            {
                target.OnStand(targetStandTime);        // ���� ����.
                targetCount -= 1;                        // ���� ���� ���� ����.
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
        Debug.Log("���� ����");
    }
}
