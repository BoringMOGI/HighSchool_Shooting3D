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
    public Bullet bulletPrefab; // �Ѿ� ������.
                                    
    int ammoCount;                  // ���� ź�� ����.
    float nextAttackTime;           // �߻� ���� �ð�.
    bool isReloading;               // ������ ���ΰ�?

    private void Start()
    {
        ammoCount = maxAmmoCount;
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

            Bullet bullet = Instantiate(bulletPrefab, muzzlePivot.position, muzzlePivot.rotation);

            Vector3 direction = bullet.transform.forward;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                direction = hit.point - bullet.transform.position;

            bullet.Shoot(direction, bulletSpeed);

            float reboundX = Random.Range(-0.5f, 0.6f) * (anim.GetBool("isAim") ? 0.5f : 1.0f);
            float reboundY = 0.94f * (anim.GetBool("isAim") ? 0.5f : 1.0f);
            mouseLook.Rebound(reboundX, reboundY);
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
    }
}
