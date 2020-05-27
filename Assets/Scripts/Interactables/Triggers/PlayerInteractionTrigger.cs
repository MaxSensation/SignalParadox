//Main author: Maximiliam Rosén

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactables.Triggers
{
    public class PlayerInteractionTrigger : MonoBehaviour
    {
        private BoxCollider interactionTriggerCollider;
        public static Action<GameObject> onInteractedEvent;

        private void Awake() => interactionTriggerCollider = GetComponent<BoxCollider>();

        public void Activate(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            foreach (var interactableCollider in Physics.OverlapBox(interactionTriggerCollider.transform.position, interactionTriggerCollider.size , transform.rotation))
                if (interactableCollider.CompareTag("Interactable"))
                    onInteractedEvent?.Invoke(interactableCollider.gameObject);
        }
    }
}