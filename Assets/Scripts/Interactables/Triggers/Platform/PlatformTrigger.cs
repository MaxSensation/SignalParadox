//Main author: Maximiliam Rosén

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables.Triggers.Platform
{
    public class PlatformTrigger : MonoBehaviour
    {
        [SerializeField] private bool triggerOnes;
        [SerializeField] private Material on;
        [SerializeField] private Material off;
        [SerializeField] private GameObject[] interactables;
        private Animator _animator;
        private MeshRenderer _meshRenderer;
        private static readonly int IsPressed = Animator.StringToHash("IsPressed");
        public UnityEvent isOn;
        public UnityEvent isOff;
        public static Action<GameObject[]> onButtonPressedEvent;
        private int objectsOnButton;

        private void Awake()
        {
            objectsOnButton = 0;
            var parent = transform.parent;
            _animator = parent.GetComponent<Animator>();
            _meshRenderer = parent.transform.Find("PlatformButton").GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") && !other.CompareTag("Enemy") && !other.CompareTag("Interactable")) return;
            Activate();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (triggerOnes) return;
            if ((!other.CompareTag("Player") && !other.CompareTag("Enemy") && !other.CompareTag("Interactable"))) return;
            Deactivate();
        }
        
        [ContextMenu("Activate")]
        private void Activate()
        {
            objectsOnButton++;
            if (objectsOnButton != 1) return;
            _animator.SetBool(IsPressed, true);
            _meshRenderer.material = @on;
            isOn.Invoke();
            onButtonPressedEvent?.Invoke(interactables);
        }
        [ContextMenu("Deactivate")]
        private void Deactivate()
        {
            objectsOnButton--;
            if (objectsOnButton != 0) return;
            _animator.SetBool(IsPressed, false);
            _meshRenderer.material = off;
            isOff.Invoke();
            onButtonPressedEvent?.Invoke(interactables);
        }
    }
}
