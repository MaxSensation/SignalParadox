using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTrigger : MonoBehaviour
{
    public static Action<GameObject> OnEnemyWithinMeleeRange;
    public static Action<GameObject> OnEnemyOutsideMeleeRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            OnEnemyWithinMeleeRange?.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            OnEnemyOutsideMeleeRange?.Invoke(other.gameObject);
        }
    }

}
