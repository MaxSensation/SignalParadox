//Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

using System;
using AI.Charger.AIStateMachine;
using Interactables.Traps;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChargerAnimatorController : MonoBehaviour
{
    private Animator animator;

    //the parent for the actual Charger object
    private GameObject charger;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        charger = transform.parent.gameObject;
        LaserController.onLaserDeath += Die;
        PatrolState.onPatrolEvent += Walk;
        ChargeState.onChargeEvent += Charge;
        ChargeState.onStunnedEvent += Stunned;
        ChargeState.onSlowChargeEvent += Idle;
        ChargeUpState.onChargeUpEvent += ChargeUp;
    }

    private void Idle(GameObject obj)
    {
        if (charger != obj) return;
        animator.SetTrigger("Idle");
        //animator.ResetTrigger("ChargeUp");
    }

    private void Stunned(GameObject obj)
    {
        if (charger != obj) return;
        animator.SetTrigger("Stunned");
        animator.ResetTrigger("ChargeUp");
    }

    private void ChargeUp(GameObject obj)
    {
        if (charger != obj) return;
        animator.SetTrigger("ChargeUp");
        animator.ResetTrigger("Walk");
    }

    private void Charge(GameObject obj)
    {
        if (charger != obj) return;
        animator.SetTrigger("Charge");
    }

    private void Die(GameObject obj)
    {
        if (charger != obj) return;
        animator.SetTrigger("Die");
    }

    private void Walk(GameObject obj)
    {
        if (charger != obj) return;
        animator.SetTrigger("Walk");
    }

    private void OnDestroy()
    {
        LaserController.onLaserDeath -= Die;
        PatrolState.onPatrolEvent -= Walk;
        ChargeState.onChargeEvent -= Charge;
        ChargeState.onStunnedEvent -= Stunned;
        ChargeUpState.onChargeUpEvent -= ChargeUp;
    }

}
