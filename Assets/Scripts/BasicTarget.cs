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

    protected void Start()
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

        OnDamaged(damage);
    }

    protected virtual void OnDamaged(int damage)
    {
        if (isAppear == false)
            return;

        if (hp <= 0)
            return;

        hp -= damage;
        if (hp <= 0)
        {
            // 점수 증가.
            OnTargetHit?.Invoke();          // 등록된 이벤트 호출.
            TargetOut();
        }
    }
    protected virtual void TargetOut()
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

