using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Animation anim;
    public Collider collider;

    private bool isAppear;              // ���� �ߴ°�?
    public bool IsAppear => isAppear;   // �ܺ� ���� (�б� ����)

    float standTime;                    // �� �ִ� �ð�.
    bool isCheckStandTime;              // �� �ִ� �ð��� üũ�ϴ°�?

    public event System.Action OnTargetHit; // �̺�Ʈ�� ��������Ʈ.
    public event System.Action OnTargetOut; // Ÿ���� �����ߴ�.

    private void Start()
    {
        //collider.enabled = false;       // ���ʿ��� �ݸ����� �����ִ�.
        TargetOut();
    }

    private void Update()
    {
        // ���� �������� �ʾҰų� üũ Ÿ�� �ð��� �ƴ϶��.
        if (IsAppear == false || isCheckStandTime == false)
            return;

        if(standTime <= Time.time)
        {
            TargetOut();
        }
    }

    public void OnStand(float standTime)
    {
        anim.Play("Target_In");        
        isAppear = true;                        // Ÿ���� ����.
        this.standTime = standTime;             // ���ִ� �ð� ����.
    }
    public void OnEndStand()
    {
        collider.enabled = true;
        standTime = standTime + Time.time;      // Time.time(���� �ð�) + ���� �ð�.
        isCheckStandTime = true;                // �̶����� ���ִ� �ð� üũ.
    }

    public void TargetHit()
    {
        // ���� ����.
        OnTargetHit?.Invoke();          // ��ϵ� �̺�Ʈ ȣ��.
        TargetOut();
    }
    private void TargetOut()
    {
        anim.Play("Target_Out");        // �������� �ִϸ��̼� ���.
        collider.enabled = false;       // Ÿ���� �������� ���� �ݸ����� ����. (�ߺ� ȣ�� ����)
        isCheckStandTime = false;       // �������� ���̶� ���ִ� �ð� üũ�� ������� ����.
    }
    public void OnEndTargetOut()
    {
        isAppear = false;                // �������� �ִϸ��̼��� ����Ǿ�� isAppear�� false�� �ȴ�.
        OnTargetOut?.Invoke();
    }    



}
