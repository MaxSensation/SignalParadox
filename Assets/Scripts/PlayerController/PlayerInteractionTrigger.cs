using System;
using UnityEngine;

namespace PlayerController
{
    public class PlayerInteractionTrigger : MonoBehaviour
    {
        public static Action<GameObject> onEnteredInteractionRange;
        public static Action<GameObject> onExitedInteractionRange;
    
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Interactable")) return;
            onEnteredInteractionRange?.Invoke(other.gameObject);
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Interactable")) return;
            onExitedInteractionRange?.Invoke(other.gameObject);
        }
    }
}
