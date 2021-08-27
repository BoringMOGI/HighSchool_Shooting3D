using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour
{
    public enum BODY_SITE
    {
        None = -1,
        Head,
        Body,
    }

    [SerializeField] BasicTarget receiver;
    [SerializeField] BODY_SITE bodySite;

    Collider hitCollider;

    void Start()
    {
        hitCollider = GetComponent<Collider>();
    }
    public void Hit()
    {
        receiver.TargetHit(bodySite);
    }
    public void ExplodHit()
    {
        receiver.TakeExplode();
    }

}
