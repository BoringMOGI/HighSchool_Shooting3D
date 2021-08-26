using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderChecker : MonoBehaviour
{
    [SerializeField] float sphereRadius;

    public Collider[] Check()
    {
        return Physics.OverlapSphere(transform.position, sphereRadius);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }


}
