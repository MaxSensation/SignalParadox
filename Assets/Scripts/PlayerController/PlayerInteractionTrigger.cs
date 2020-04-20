using UnityEngine;

namespace PlayerController
{
    public class PlayerInteractionTrigger : MonoBehaviour
    {
        public delegate void OnEnteredInteractionRange(GameObject gameObject);
        public delegate void OnExitedInteractionRange(GameObject gameObject);
        public static event OnEnteredInteractionRange onEnteredInteractionRange;
        public static event OnExitedInteractionRange onExitedInteractionRange;
    
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
