using AI.Charger.AIStateMachine;
using UnityEngine;

namespace Interactables.Triggers.Events
{
    public class GlassWallTrigger : MonoBehaviour
    {
        private BoxCollider[] colliers;
        private Animator animator;
        private AudioSource audioSource;
        private bool isDestroyable;
        
        private void Awake()
        {
            colliers = GetComponents<BoxCollider>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            ChargeState.onChargeEvent += charger => isDestroyable = true;
            ChargeState.onStunnedEvent += charger => isDestroyable = false;
            ChargeState.onSlowChargeEvent += charger => isDestroyable = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isDestroyable) return;
            if (other.CompareTag("Enemy"))
                DestoryWall();
        }

        private void DestoryWall()
        {
            for (var i = 0; i < colliers.Length; i++)
            {
                colliers[i].enabled = false;
            }
            animator.SetTrigger("GlassBreak");
            audioSource.Play();    
        }
    }
}
