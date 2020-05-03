using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    public class PlayerInteractionTrigger : MonoBehaviour
    {
        private bool _playerInteracted;
        public static Action<GameObject> onInteracted;
        
        private void OnTriggerStay(Collider other)
        {
            if (!_playerInteracted || !other.CompareTag("Interactable")) return;
            onInteracted?.Invoke(other.gameObject);
            _playerInteracted = false;
        }

        public void PressButton(InputAction.CallbackContext context)
        {
            if (context.performed)
                _playerInteracted = true;
        }
    }
}
