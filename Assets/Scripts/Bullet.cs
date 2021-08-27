using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform bulletHolePrefab;
    public LayerMask groundLayer;

    Rigidbody rigid;

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 충돌체가 Grond Layer를 가지고 있을 경우.
        if ((groundLayer & 1 << collision.gameObject.layer) != 0)
        {
            // 탄흔 만들기.
            Transform bulletHole = Instantiate(bulletHolePrefab, transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
            bulletHole.SetParent(collision.transform);
        }
        if (collision.gameObject.tag == "HitBox")
        {
            Hitbox hitBox = collision.gameObject.GetComponent<Hitbox>();
            if (hitBox != null)
            {
                hitBox.Hit();
            }
        }

        Destroy(gameObject);
    }

    public void Shoot(Vector3 direction, float bulletSpeed)
    {
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = direction * bulletSpeed;
    }
}
