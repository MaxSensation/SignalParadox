//Main author: Maximiliam Rosén

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables.Platform
{
    public class PlatformTrigger : MonoBehaviour
    {
        [SerializeField] private bool triggerOnce;
        [SerializeField] private Material on ,off;
        [SerializeField] private GameObject[] interactables;
        private string[] activationTags;
        private Animator animator;
        private BoxCollider triggerArea;
        private MeshRenderer meshRenderer;
        private static readonly int IsPressed = Animator.StringToHash("IsPressed");
        public UnityEvent isOn, isOff;
        public static Action<GameObject[]> onButtonPressedEvent;
        private enum State { On, Off }
        private State currentState;
        
        private void Awake()
        {
            currentState = State.Off;
            activationTags = new[] {"Player", "Enemy", "Interactable"};
            triggerArea = GetComponent<BoxCollider>();
            var parent = transform.parent;
            animator = parent.GetComponent<Animator>();
            meshRenderer = parent.transform.Find("PlatformButton").GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (activationTags.Contains(other.tag) && currentState == State.Off)
                Activate();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (triggerOnce || !activationTags.Contains(other.tag)) return;
            if (!Physics.OverlapBox(transform.position, triggerArea.bounds.extents, transform.rotation).Any(col => activationTags.Contains(col.tag)))
                Deactivate();
        }
        
        private void Activate()
        {
            currentState = State.On;
            animator.SetBool(IsPressed, true);
            meshRenderer.material = on;
            isOn.Invoke();
            onButtonPressedEvent?.Invoke(interactables);
        }

        private void Deactivate()
        {
            currentState = State.Off;
            animator.SetBool(IsPressed, false);
            meshRenderer.material = off;
            isOff.Invoke();
            onButtonPressedEvent?.Invoke(interactables);
        }
    }
}
