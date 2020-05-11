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
    }

    private void Die(GameObject obj)
    {
        if (bodytrapper != obj) return;
            animator.SetTrigger("Died");
    }

    private void OnDestroy()
    {
        LaserController.onLaserDeath -= Die;
        SteamController.onSteamDamage -= Die;
    }
}
