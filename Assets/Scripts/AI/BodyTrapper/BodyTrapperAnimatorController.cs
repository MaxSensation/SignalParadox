// Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

using AI.BodyTrapper.AIStateMachine;
using Interactables.Traps;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BodyTrapperAnimatorController : MonoBehaviour
{
    private Animator animator;
    //the parent for the actual bodytrapper object
    private GameObject bodytrapper;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bodytrapper = transform.parent.gameObject;
        LaserController.onLaserDeath += Die;
        SteamController.onSteamDamage += Die;
        JumpState.onJumpEvent += Jump;
        StunState.onLandEvent += Land;
        HuntState.onHuntEvent += Hunting;
        SeekingState.onIdleEvent += Stopped;
        SeekingState.onMovingEvent += Walk;
    }
    
    private void OnDestroy()
    {
        LaserController.onLaserDeath -= Die;
        SteamController.onSteamDamage -= Die;
        JumpState.onJumpEvent -= Jump;
        StunState.onLandEvent -= Land;
        HuntState.onHuntEvent -= Hunting;
        SeekingState.onIdleEvent -= Stopped;
        SeekingState.onMovingEvent -= Walk;
    }

    private void Walk(GameObject obj)
    {
        if (bodytrapper != obj) return;
        animator.SetBool("Moving", true);
    }

    private void Stopped(GameObject obj)
    {
        if (bodytrapper != obj) return;
        animator.SetBool("Moving", false);
    }

    private void Hunting(GameObject obj)
    {
        if (bodytrapper != obj) return;
        animator.SetTrigger("Hunting");
    }

    private void Land(GameObject obj)
    {
        if (bodytrapper != obj) return;
        animator.SetTrigger("Landed");
    }

    private void Jump(GameObject obj)
    {
        if (bodytrapper != obj) return;
        animator.SetTrigger("Jump");
    }

    private void Die(GameObject obj)
    {
        if (bodytrapper != obj) return;
            animator.SetTrigger("Died");
    }
}
