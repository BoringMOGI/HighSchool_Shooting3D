using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Attack")]
    public float attackRate;        // 연사 속도.    
    public float bulletSpeed;       // 탄약 속도.
    public int maxAmmoCount;        // 탄약 개수.

    [Header("Animation")]
    public Animator anim;           // 제어 애니메이션.
    public AudioSource fireSE;      // 발사 효과음.
    public AudioSource reloadSE;    // 재장전 효과음.
    public MouseLook mouseLook;     

    [Header("Bullet")]
    public Transform muzzlePivot;   // 총구의 위치.
    public Bullet bulletPrefab; // 총알 프리팹.
                                    
    int ammoCount;                  // 남은 탄약 개수.
    float nextAttackTime;           // 발사 가능 시간.
    bool isReloading;               // 재장전 중인가?

    private void Start()
    {
        ammoCount = maxAmmoCount;
    }

    public void Fire()
    {
        if (nextAttackTime <= Time.time && ammoCount > 0 && isReloading == false)
        {
            // 탄약을 발사한다.
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
