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
        [SerializeField] private Vector3 checkSize;
        [SerializeField] private float force;
        [SerializeField] private float distanceToPushLocation;
        public static Action<IPushable> onPushStateEvent;
        public UnityEvent onPushEvent, onStopPushEvent;
        private Vector3 halfCheckSize, pushDirection;
        private BoxCollider boxCollider;
        private Rigidbody boxRigidbody;
        private Vector3[] pushingLocations;
        private bool isPushing;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
            boxRigidbody = GetComponent<Rigidbody>();
            halfCheckSize = new Vector3(checkSize.x / 2, checkSize.y / 2, checkSize.z / 2);
            GetPushLocations();
            InteractionTrigger.onInteractedEvent += HandleInteract;
        }

        private void GetPushLocations()
        {
            var boxTransform = transform;
            var forward = boxTransform.forward;
            var right = boxTransform.right;
            var position = boxTransform.position;
            var distanceFromCenter = boxCollider.size.x / 2 + distanceToPushLocation;
            pushingLocations = new Vector3[]
            {
                position + forward * (distanceFromCenter + checkSize.z/2),
                position + -forward * (distanceFromCenter + checkSize.z/2),
                position + right * (distanceFromCenter + checkSize.x/2),
                position + -right * (distanceFromCenter + checkSize.x/2)
            };
        }

        private void OnDestroy() => InteractionTrigger.onInteractedEvent -= HandleInteract;

        private void HandleInteract(GameObject interactable)
        {
            if (interactable == gameObject)
                onPushStateEvent?.Invoke(this);
        }

        private void FixedUpdate()
        {
            if (!isPushing) return;
            boxRigidbody.AddForce(pushDirection.normalized * (force * Time.deltaTime));
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
            GetPushLocations();
            var bestLocation = Vector3.zero;
            var bestDistance = float.PositiveInfinity;
            foreach (var location in pushingLocations)
            {
                var distance = Vector3.Distance(pusherLocation, location);
                if (!(distance < bestDistance)) continue;
                bestDistance = distance;
                bestLocation = location;
                var position = transform.position;
                pushDirection = (new Vector3(position.x, pusherLocation.y, position.z) - pusherLocation).normalized;
            }
            return CheckIfLocationIsValid(bestLocation) ? new Vector3(bestLocation.x, pusherLocation.y, bestLocation.z) : Vector3.zero;
        }

        private bool CheckIfLocationIsValid(Vector3 location)
        {
            var layerMask = LayerMask.NameToLayer("Colliders");
            return Physics.OverlapBox(location, halfCheckSize).Where(c => c.gameObject.layer.Equals(layerMask)).ToList().Count == 0;
        }

        public Transform GetPushableTransform()
        {
            return transform;
        }
    }
}
