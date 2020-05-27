//Main author: Maximiliam Rosén

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables.Triggers.Platform
{
    public class PlatformTrigger : MonoBehaviour
    {
        [SerializeField] private bool triggerOnce;
        [SerializeField] private Material on ,off;
        [SerializeField] private GameObject[] interactables;
        private Animator animator;
        private MeshRenderer meshRenderer;
        private static readonly int IsPressed = Animator.StringToHash("IsPressed");
        public UnityEvent isOn, isOff;
        public static Action<GameObject[]> onButtonPressedEvent;
        private int objectsOnButton;

        private void Awake()
        {
            var parent = transform.parent;
            animator = parent.GetComponent<Animator>();
            meshRenderer = parent.transform.Find("PlatformButton").GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") && !other.CompareTag("Enemy") && !other.CompareTag("Interactable")) return;
            Activate();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (triggerOnce || ((!other.CompareTag("Player") && !other.CompareTag("Enemy") && !other.CompareTag("Interactable")))) return;
            Deactivate();
        }
        
        private void Activate()
        {
            objectsOnButton++;
            if (objectsOnButton != 1) return;
            animator.SetBool(IsPressed, true);
            meshRenderer.material = @on;
            isOn.Invoke();
            onButtonPressedEvent?.Invoke(interactables);
        }

        private void Deactivate()
        {
            objectsOnButton--;
            if (objectsOnButton != 0) return;
            animator.SetBool(IsPressed, false);
            meshRenderer.material = off;
            isOff.Invoke();
            onButtonPressedEvent?.Invoke(interactables);
        }
    }
}
