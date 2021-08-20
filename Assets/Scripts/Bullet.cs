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
            // ÅºÈç ¸¸µé±â.
            Instantiate(bulletHolePrefab, transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
        }
        else if(collision.gameObject.tag == "Target")
        {
            Target target = collision.gameObject.GetComponent<Target>();
            if(target != null)
            {
                target.TargetHit();
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
