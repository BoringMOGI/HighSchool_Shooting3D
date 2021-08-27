using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Target : BasicTarget
{
    float moveSpeed;                    // 이동 속도.
    bool isMoveRight;                   // 오른쪽으로 이동중인가?

    Vector3 minPosition;                // 최소 거리.
    Vector3 maxPosition;                // 최대 거리.
    Vector3 destination;                // 목적지.

    private void Update()
    {
        // 만약 등장하지 않았거나 체크 타임 시간이 아니라면.
        if (IsAppear == false || isCheckStandTime == false)
            return;

        if (standTime <= Time.time)
        {
            TargetOut();
        }

        // 움직임.
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        if(transform.position == destination)
        {
            isMoveRight = !isMoveRight;         // 방향 전환.
            destination = isMoveRight ? maxPosition : minPosition;
        }
    }

    public void OnStand(GameMode.TargetInfo info, Vector3 minPosition, Vector3 maxPosition)
    {
        OnStand();

        this.standTime = info.standTime;                // 서있는 시간 대입.
        this.moveSpeed = info.moveSpeed;
        this.minPosition = minPosition;
        this.maxPosition = maxPosition;
        hp = info.targetHp;

        isMoveRight = Random.Range(0.0f, 1.0f) < 0.5f;          // 50% 확률로 true or false.
        destination = isMoveRight ? maxPosition : minPosition;  // 목적지 설정.

        transform.position = new Vector3(Random.Range(minPosition.x, maxPosition.x), minPosition.y, minPosition.z);
    }
}
