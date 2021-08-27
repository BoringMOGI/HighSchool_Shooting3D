using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] GameObject explsionEffect;
    [SerializeField] float explodeRadius;

    public void Throw(Vector3 direction, float throwForce)
    {
        rigid.AddForce(direction * throwForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Explode();
    }

    void Explode()
    {
        Instantiate(explsionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider collider in colliders)
        {
            Hitbox hitBox = collider.GetComponent<Hitbox>();
            if(hitBox != null)
            {
                hitBox.ExplodHit();
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }
}
