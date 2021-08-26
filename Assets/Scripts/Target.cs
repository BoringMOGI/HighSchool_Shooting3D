using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTarget : MonoBehaviour
{
    public event System.Action OnTargetHit; // 이벤트형 델리게이트.
    public event System.Action OnTargetOut; // 타겟이 퇴장했다.

    public Animation anim;

    protected bool isAppear;              // 등장 했는가?
    public bool IsAppear => isAppear;   // 외부 공개 (읽기 전용)

    protected float standTime;                    // 서 있는 시간.
    protected bool isCheckStandTime;              // 서 있는 시간을 체크하는가?
    protected int hp;                             // 타겟의 체력.

    private void Start()
    {
        //collider.enabled = false;       // 최초에는 콜리더가 꺼져있다.
        TargetOut();
    }

    public virtual void OnStand()
    {
        anim.Play("Target_In");
        isAppear = true;                                // 타겟의 등장.
        hp = 4;
    }
    public void OnEndStand()
    {
        standTime = standTime + Time.time;      // Time.time(현재 시간) + 유지 시간.
        isCheckStandTime = true;                // 이때부터 서있는 시간 체크.
    }

    public void TakeExplode()
    {
        int damage = 200;
        OnDamaged(damage);
    }
    public void TargetHit(Hitbox.BODY_SITE site)
    {
        int damage = 0;
        if (site == Hitbox.BODY_SITE.Head)
        {
            damage = 100;
        }
        else if (site == Hitbox.BODY_SITE.Body)
        {
            damage = 1;
        }

        Debug.Log(string.Format("HIT {0} : {1}({2})", name, damage, site.ToString()));
        OnDamaged(damage);
    }
    private void OnDamaged(int damage)
    {
        if (isAppear == false)
            return;

        hp -= damage;
        if (hp <= 0)
        {
            // 점수 증가.
            OnTargetHit?.Invoke();          // 등록된 이벤트 호출.
            TargetOut();
        }
    }
    protected void TargetOut()
    {
        anim.Play("Target_Out");        // 쓰러지는 애니메이션 재생.
        isCheckStandTime = false;       // 쓰러지는 중이라 서있는 시간 체크를 허용하지 않음.
    }
    public void OnEndTargetOut()
    {
        isAppear = false;                // 쓰러지는 애니메이션이 종료되어야 isAppear가 false가 된다.
        OnTargetOut?.Invoke();
    }
}

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
