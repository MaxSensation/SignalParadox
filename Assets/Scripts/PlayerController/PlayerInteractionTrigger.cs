using System;
using UnityEngine;

namespace PlayerController
{
    public class PlayerInteractionTrigger : MonoBehaviour
    {
        private bool _playerinteracted;
        public static Action<GameObject> onInteracted;
        
        private void OnTriggerStay(Collider other)
        {
            if (_playerinteracted && other.CompareTag("Interactable"))
            {
                onInteracted?.Invoke(other.gameObject);
                _playerinteracted = false;
            }
        }

        public void PressButton()
        {
            _playerinteracted = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PressButton();
            }
        }
    }
}
