using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTarget : MonoBehaviour
{
    public event System.Action OnTargetHit; // �̺�Ʈ�� ��������Ʈ.
    public event System.Action OnTargetOut; // Ÿ���� �����ߴ�.

    public Animation anim;

    protected bool isAppear;              // ���� �ߴ°�?
    public bool IsAppear => isAppear;   // �ܺ� ���� (�б� ����)

    protected float standTime;                    // �� �ִ� �ð�.
    protected bool isCheckStandTime;              // �� �ִ� �ð��� üũ�ϴ°�?
    protected int hp;                             // Ÿ���� ü��.

    protected void Start()
    {
        //collider.enabled = false;       // ���ʿ��� �ݸ����� �����ִ�.
        TargetOut();
    }

    public virtual void OnStand()
    {
        anim.Play("Target_In");
        isAppear = true;                                // Ÿ���� ����.
        hp = 4;
    }
    public void OnEndStand()
    {
        standTime = standTime + Time.time;      // Time.time(���� �ð�) + ���� �ð�.
        isCheckStandTime = true;                // �̶����� ���ִ� �ð� üũ.
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
            // ���� ����.
            OnTargetHit?.Invoke();          // ��ϵ� �̺�Ʈ ȣ��.
            TargetOut();
        }
    }
    protected virtual void TargetOut()
    {
        anim.Play("Target_Out");        // �������� �ִϸ��̼� ���.
        isCheckStandTime = false;       // �������� ���̶� ���ִ� �ð� üũ�� ������� ����.
    }
    public void OnEndTargetOut()
    {
        isAppear = false;                // �������� �ִϸ��̼��� ����Ǿ�� isAppear�� false�� �ȴ�.
        OnTargetOut?.Invoke();
    }
}

