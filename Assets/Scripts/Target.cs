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

    private void Start()
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
            // ���� ����.
            OnTargetHit?.Invoke();          // ��ϵ� �̺�Ʈ ȣ��.
            TargetOut();
        }
    }
    protected void TargetOut()
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
