using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Target : BasicTarget
{
    float moveSpeed;                    // �̵� �ӵ�.
    bool isMoveRight;                   // ���������� �̵����ΰ�?

    Vector3 minPosition;                // �ּ� �Ÿ�.
    Vector3 maxPosition;                // �ִ� �Ÿ�.
    Vector3 destination;                // ������.

    private void Update()
    {
        // ���� �������� �ʾҰų� üũ Ÿ�� �ð��� �ƴ϶��.
        if (IsAppear == false || isCheckStandTime == false)
            return;

        if (standTime <= Time.time)
        {
            TargetOut();
        }

        // ������.
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        if(transform.position == destination)
        {
            isMoveRight = !isMoveRight;         // ���� ��ȯ.
            destination = isMoveRight ? maxPosition : minPosition;
        }
    }

    public void OnStand(GameMode.TargetInfo info, Vector3 minPosition, Vector3 maxPosition)
    {
        OnStand();

        this.standTime = info.standTime;                // ���ִ� �ð� ����.
        this.moveSpeed = info.moveSpeed;
        this.minPosition = minPosition;
        this.maxPosition = maxPosition;
        hp = info.targetHp;

        isMoveRight = Random.Range(0.0f, 1.0f) < 0.5f;          // 50% Ȯ���� true or false.
        destination = isMoveRight ? maxPosition : minPosition;  // ������ ����.

        transform.position = new Vector3(Random.Range(minPosition.x, maxPosition.x), minPosition.y, minPosition.z);
    }
}
