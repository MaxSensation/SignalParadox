//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using AI.Charger.AIStateMachine;
using UnityEngine;
using System;
namespace Interactables.Triggers.Events
{
    public class GlassWallTrigger : MonoBehaviour
    {
        [SerializeField] private float chargerLookAngle = 0.5f;
        private BoxCollider[] colliers;
        private Animator animator;
        private AudioSource audioSource;
        private bool isDestroyable;
        private Vector3 glassForward;

        public static Action onBrokenEvent;
        
        private void Awake()
        {
            colliers = GetComponents<BoxCollider>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            glassForward = new Vector3(transform.forward.x, 0, transform.forward.z);

            ChargeState.onStunnedEvent += charger => isDestroyable = false;
            ChargeState.onSlowChargeEvent += charger => isDestroyable = false;
            ChargeState.onFastChargeEvent += charger => isDestroyable = true;
        }

        private void OnDestroy()
        {
            ChargeState.onStunnedEvent -= charger => isDestroyable = false;
            ChargeState.onSlowChargeEvent -= charger => isDestroyable = false;
            ChargeState.onFastChargeEvent -= charger => isDestroyable = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isDestroyable) return;
            var chargerForward = new Vector3(other.transform.forward.x, 0, other.transform.forward.z);
            Debug.Log(Vector3.Dot(glassForward, chargerForward));
            if (other.CompareTag("Enemy") && Vector3.Dot(glassForward, chargerForward) > chargerLookAngle)
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
            onBrokenEvent?.Invoke();
        }
    }
}
