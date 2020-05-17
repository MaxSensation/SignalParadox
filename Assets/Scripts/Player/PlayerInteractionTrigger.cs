//Main author: Maximiliam Rosén

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteractionTrigger : MonoBehaviour
    {
        private BoxCollider _collider;
        public static Action<GameObject> onInteractedEvent;

        private void Awake() => _collider = GetComponent<BoxCollider>();

        public void Activate(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            foreach (var collider in Physics.OverlapBox(_collider.transform.position, _collider.size , transform.rotation))
            {
                if (collider.CompareTag("Interactable"))
                    onInteractedEvent?.Invoke(collider.gameObject);
            }
        }
    }
}
