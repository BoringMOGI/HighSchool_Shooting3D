using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggetTarget : BasicTarget
{
    [SerializeField] Transform checkPivot;
    [SerializeField] float distance;
    [SerializeField] float radius;
    [SerializeField] LayerMask checkMask;
    bool isRay = true;

    SimulateManager manager;
    
    protected new void Start()
    {
        manager = FindObjectOfType<SimulateManager>();
        anim.Play("TriggetTarget_Start");
    }
    public override void OnStand()
    {
        anim.Play("TriggetTarget_In");
        isAppear = true;                                // ≈∏∞Ÿ¿« µÓ¿Â.
        hp = 4;
    }
    protected override void TargetOut()
    {
        anim.Play("Target_Out");
        if (manager != null)
            manager.OnTargetOut();
    }

    private void Update()
    {
        if (!isRay)
            return;

        RaycastHit hit;
        if(Physics.SphereCast(checkPivot.position, radius, transform.forward * -1f, out hit, distance, checkMask))
        {
            isRay = false;
            OnStand();
        }
    }

    private void OnDrawGizmos()
    {   
        if (isRay)
        {
            Gizmos.DrawRay(checkPivot.position, transform.forward * -1f * distance);
            Gizmos.DrawWireSphere(checkPivot.position + transform.forward * -1f * distance, radius);
        }
    }
}
