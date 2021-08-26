using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTarget : BasicTarget
{
    [SerializeField] Collider checkCollider;
    [SerializeField] LayerMask checkMasks;

    private void OnTriggerEnter(Collider other)
    {
        if((checkMasks & other.gameObject.layer) != 0)
        {
            checkCollider.enabled = false;
            anim.Play("Target_In");
        }
    }
}
