//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using System.Linq;
using Interactables.Triggers.EntitiesTrigger;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables.Pushables
{
    public class PushableBox : MonoBehaviour, IPushable
    {
        [SerializeField] private Vector3 triggerCheckSize;
        [SerializeField] private float pushForce;
        [SerializeField] private float playerDistanceToPushArea;
        private Vector3 halfTriggerCheckSize, pushDirection;
        private BoxCollider triggerCollider;
        private Rigidbody boxRigidbody;
        private Vector3[] pushableLocations;
        private bool isPushing;
        public UnityEvent onPushEvent, onStopPushEvent;
        public static Action<IPushable> onPushStateEvent;

        private void Awake()
        {
            triggerCollider = GetComponent<BoxCollider>();
            boxRigidbody = GetComponent<Rigidbody>();
            halfTriggerCheckSize = new Vector3(triggerCheckSize.x / 2, triggerCheckSize.y / 2, triggerCheckSize.z / 2);
            GeneratePushLocations();
            InteractionTrigger.onInteractedEvent += HandleInteract;
        }

        private void OnDestroy() => InteractionTrigger.onInteractedEvent -= HandleInteract;

        private void FixedUpdate()
        {
            if (!isPushing) return;
            boxRigidbody.AddForce(pushDirection.normalized * (pushForce * Time.deltaTime));
        }

        private void GeneratePushLocations()
        {
            var boxTransform = transform;
            var forward = boxTransform.forward;
            var right = boxTransform.right;
            var position = boxTransform.position;
            var distanceFromCenter = triggerCollider.size.x / 2 + playerDistanceToPushArea;
            pushableLocations = new Vector3[]
            {
                position + forward * (distanceFromCenter + triggerCheckSize.z/2),
                position + -forward * (distanceFromCenter + triggerCheckSize.z/2),
                position + right * (distanceFromCenter + triggerCheckSize.x/2),
                position + -right * (distanceFromCenter + triggerCheckSize.x/2)
            };
        }

        private void HandleInteract(GameObject interactable)
        {
            if (interactable == gameObject)
                onPushStateEvent?.Invoke(this);
        }

        public void Pushing()
        {
            if (!isPushing)
                onPushEvent?.Invoke();
            isPushing = true;
        }

        public void NotPushing()
        {
            onStopPushEvent?.Invoke();
            isPushing = false;
        }

        public Vector3 GetPushLocation(Vector3 pusherLocation)
        {
            GeneratePushLocations();
            var bestLocation = Vector3.zero;
            var bestDistance = float.PositiveInfinity;
            for (int i = 0; i < pushableLocations.Length; i++)
            {
                var distance = Vector3.Distance(pusherLocation, pushableLocations[i]);
                if (!(distance < bestDistance)) continue;
                bestDistance = distance;
                bestLocation = pushableLocations[i];
                var position = transform.position;
                pushDirection = (new Vector3(position.x, pusherLocation.y, position.z) - pusherLocation).normalized;
            }
            return CheckIfLocationIsValid(bestLocation) ? new Vector3(bestLocation.x, pusherLocation.y, bestLocation.z) : Vector3.zero;
        }

        private bool CheckIfLocationIsValid(Vector3 location)
        {
            var layerMask = LayerMask.NameToLayer("Colliders");
            return Physics.OverlapBox(location, halfTriggerCheckSize).Where(c => c.gameObject.layer.Equals(layerMask)).ToList().Count == 0;
        }

        public Transform GetPushableTransform() => transform;
    }
}
