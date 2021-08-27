using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventer : MonoBehaviour
{
    [SerializeField] UnityEvent OnTerggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        OnTerggerEnter?.Invoke();
    }
}
