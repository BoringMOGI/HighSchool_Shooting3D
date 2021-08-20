using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Attack")]
    public float attackRate;        // ���� �ӵ�.    
    public float bulletSpeed;       // ź�� �ӵ�.
    public int maxAmmoCount;        // ź�� ����.

    [Header("Animation")]
    public Animator anim;           // ���� �ִϸ��̼�.
    public AudioSource fireSE;      // �߻� ȿ����.
    public AudioSource reloadSE;    // ������ ȿ����.
    public MouseLook mouseLook;     

    [Header("Bullet")]
    public Transform muzzlePivot;   // �ѱ��� ��ġ.
    public Bullet bulletPrefab;     // �Ѿ� ������.
    public LayerMask contactLayer;  // �浹 �Ǵ� ���̾�.

    [Header("UI")]
    public StateInfoUI stateInfoUi;
                                    
    int ammoCount;                  // ���� ź�� ����.
    float nextAttackTime;           // �߻� ���� �ð�.
    bool isReloading;               // ������ ���ΰ�?

    private void Start()
    {
        ammoCount = maxAmmoCount;
        stateInfoUi.SetBulletText(ammoCount, 150);
    }

    float hp = 100f;
    float maxHp = 100f;
    private void Update()
    {
        hp -= Time.deltaTime;
        stateInfoUi.SetHpImage(hp, maxHp);
    }

    public void Fire()
    {
        if (nextAttackTime <= Time.time && ammoCount > 0 && isReloading == false)
        {
            // ź���� �߻��Ѵ�.
            anim.SetTrigger("onFire");
            fireSE.Play();
            ammoCount -= 1;
            nextAttackTime = Time.time + attackRate;

            // Time.timeScale = 0.2f;

            Bullet bullet = Instantiate(bulletPrefab, muzzlePivot.position, muzzlePivot.rotation);
            stateInfoUi.SetBulletText(ammoCount, 150);

            Vector3 bulletDirection = bullet.transform.forward;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1000f, contactLayer))
            {
                bulletDirection = hit.point - muzzlePivot.position; // �Ѿ��� ���ư����� ���� ����.
            }

            bullet.Shoot(bulletDirection, bulletSpeed);             // �ش� �������� �Ѿ��� ���.


            /*
            float reboundX = Random.Range(-0.5f, 0.6f) * (anim.GetBool("isAim") ? 0.5f : 1.0f);
            float reboundY = 0.94f * (anim.GetBool("isAim") ? 0.5f : 1.0f);
            mouseLook.Rebound(reboundX, reboundY);
            */


        }                
    }

    public void Reload()
    {
        if (isReloading)
            return;

        isReloading = true;
        reloadSE.Play();

        anim.SetTrigger("onReload");
    }

    private void OnEndReload()
    {
        isReloading = false;
        ammoCount = maxAmmoCount;
        stateInfoUi.SetBulletText(ammoCount, 150);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(muzzlePivot.position, muzzlePivot.forward * 100f);

    }
}
