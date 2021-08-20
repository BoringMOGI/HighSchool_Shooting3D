using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Animation anim;
    public Collider collider;
    
    private bool isAppear;              // ���� �ߴ°�?
    public bool IsAppear => isAppear;   // �ܺ� ���� (�б� ����)

    public event System.Action OnTargetHit; // �̺�Ʈ�� ��������Ʈ.
    public event System.Action OnTargetOut; // Ÿ���� �����ߴ�.

    float moveSpeed;                    // �̵� �ӵ�.
    float standTime;                    // �� �ִ� �ð�.
    bool isCheckStandTime;              // �� �ִ� �ð��� üũ�ϴ°�?
    bool isMoveRight;                   // ���������� �̵����ΰ�?

    Vector3 minPosition;                // �ּ� �Ÿ�.
    Vector3 maxPosition;                // �ִ� �Ÿ�.
    Vector3 destination;                // ������.

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
        anim.Play("Target_In");        
        isAppear = true;                                // Ÿ���� ����.

        this.standTime = info.standTime;                // ���ִ� �ð� ����.
        this.moveSpeed = info.moveSpeed;
        this.minPosition = minPosition;
        this.maxPosition = maxPosition;

        isMoveRight = Random.Range(0.0f, 1.0f) < 0.5f;          // 50% Ȯ���� true or false.
        destination = isMoveRight ? maxPosition : minPosition;  // ������ ����.

        transform.position = new Vector3(Random.Range(minPosition.x, maxPosition.x), minPosition.y, minPosition.z);
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
