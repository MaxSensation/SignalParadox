using System;
using UnityEngine;

namespace PlayerController
{
    public class PlayerInteractionTrigger : MonoBehaviour
    {
        private bool _playerinteracted;
        public static Action<GameObject> onPressedButton;
        
        private void OnTriggerStay(Collider other)
        {
            if (_playerinteracted && other.CompareTag("Button"))
            {
                Debug.Log("Found Button");
                onPressedButton?.Invoke(other.gameObject);
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
