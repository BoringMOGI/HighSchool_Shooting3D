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
        // �浹�� �浹ü�� Grond Layer�� ������ ���� ���.
        if ((groundLayer & 1 << collision.gameObject.layer) != 0)
        {
            // ź�� �����.
            Instantiate(bulletHolePrefab, transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
        }
        else if (collision.gameObject.tag == "HitBox")
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
