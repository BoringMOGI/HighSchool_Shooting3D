using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Target")]
    public Transform targetParent;      // Ÿ���� �θ� ������Ʈ.
    public float targetAppearRate;      // ���� �����ϴ� �ֱ�.
    public float targetStandTime;       // Ÿ���� ���ִ� �ð�.

    [Header("Count down")]
    public CountDownUI countDownUi;     // ī��Ʈ �ٿ� UI.
    public float maxCountDown;          // �� ī��Ʈ �ٿ� �ð�.
    private float countDown;            // ī��Ʈ �ٿ� �ð�.
    
    Target[] allTarget;                 // ��� Ÿ�� ������Ʈ �迭.
    float nextTargetTime;               // �������� ���� �����ϴ� �ð�.

    // Start is called before the first frame update
    void Start()
    {
        countDown = maxCountDown;
        PlayerController.isStopController = true;

        // targetParent������ �ִ� �ڽĵ� �� Target�� ���� ������Ʈ�� ���� �˻��Ѵ�.
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

        // ���� ����.
        OnUpdateTarget();
    }

    void OnStartGame()
    {
        PlayerController.isStopController = false;
        nextTargetTime = Time.time + targetAppearRate;
    }
    void OnUpdateTarget()
    {
        // ������ Ÿ���� �����Ų��.
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
