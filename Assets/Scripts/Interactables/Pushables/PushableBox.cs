﻿//Main author: Maximiliam Rosén

using System;
using System.Linq;
using PlayerController;
using UnityEngine;

namespace Interactables.Pushables
{
    public class PushableBox : MonoBehaviour, IPushable
    {
        [SerializeField] private Vector3 checkSize;
        [SerializeField] private float force;
        [SerializeField] private float distanceToPushLocation;
        private Vector3 _halfCheckSize;
        public static Action<IPushable> onPushStateEvent;
        private BoxCollider _collider;
        private Rigidbody _rigidbody;
        private Vector3[] _pushingLocations;
        private Vector3 _pushDirection;
        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _rigidbody = GetComponent<Rigidbody>();
            _halfCheckSize = new Vector3(checkSize.x/2, checkSize.y/2, checkSize.z/2);
            GetPushLocations();
            PlayerInteractionTrigger.onInteractedEvent += HandleInteract;
        }

        private void GetPushLocations()
        {
            var boxTransform = transform;
            var forward = boxTransform.forward;
            var right = boxTransform.right;
            var position = boxTransform.position;
            var distanceFromCenter = _collider.size.x/2 + distanceToPushLocation;
            _pushingLocations = new Vector3[]
            {
                position + forward * (distanceFromCenter + checkSize.z/2),
                position + -forward * (distanceFromCenter + checkSize.z/2),
                position + right * (distanceFromCenter + checkSize.x/2),
                position + -right * (distanceFromCenter + checkSize.x/2)
            };
        }

        private void OnDestroy()
        {
            PlayerInteractionTrigger.onInteractedEvent -= HandleInteract;
        }

        private void HandleInteract(GameObject interactable)
        {
            if (interactable == gameObject)
                onPushStateEvent?.Invoke(this);
        }

        public void Push()
        {
            _rigidbody.AddForce(_pushDirection * (force * Time.deltaTime));
        }

        public Vector3 GetPushLocation(Vector3 pusherLocation)
        {
            GetPushLocations();
            var bestLocation = Vector3.zero;
            var bestDistance = float.PositiveInfinity;
            foreach (var location in _pushingLocations)
            {
                var distance = Vector3.Distance(pusherLocation, location);
                if (!(distance < bestDistance)) continue;
                bestDistance = distance;
                bestLocation = location;
                var position = transform.position;
                _pushDirection = (new Vector3(position.x, pusherLocation.y, position.z) - pusherLocation).normalized;
            }
            return CheckIfLocationIsValid(bestLocation) ? new Vector3(bestLocation.x, pusherLocation.y, bestLocation.z) : Vector3.zero;
        }

        private bool CheckIfLocationIsValid(Vector3 location)
        {
            var layerMask = LayerMask.NameToLayer("Colliders");
            return Physics.OverlapBox(location, _halfCheckSize).Where(c => c.gameObject.layer.Equals(layerMask)).ToList().Count == 0;
        }

        public Transform GetPushableTransform()
        {
            return transform;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (_pushingLocations == null) return;
            foreach (var location in _pushingLocations)
            {
                Gizmos.DrawWireCube(location, checkSize);
            }
            
        }
    }
}