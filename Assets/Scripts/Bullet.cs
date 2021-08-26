using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform bulletHolePrefab;

    Rigidbody rigid;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Normal")
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
