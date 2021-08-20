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
    public Bullet bulletPrefab;     // 총알 프리팹.
    public LayerMask contactLayer;  // 충돌 되는 레이어.

    [Header("UI")]
    public StateInfoUI stateInfoUi;
                                    
    int ammoCount;                  // 남은 탄약 개수.
    float nextAttackTime;           // 발사 가능 시간.
    bool isReloading;               // 재장전 중인가?

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
            // 탄약을 발사한다.
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
                bulletDirection = hit.point - muzzlePivot.position; // 총알이 나아가야할 방향 수정.
            }

            bullet.Shoot(bulletDirection, bulletSpeed);             // 해당 방향으로 총알을 쏜다.


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
