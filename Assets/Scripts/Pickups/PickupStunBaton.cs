using System;
using UnityEngine;

namespace Pickups
{
    public class PickupStunBaton : MonoBehaviour
    {
        public static Action onStunBatonPickup;
        private void OnTriggerEnter(Collider other)
        {
            if (other && other.CompareTag("Player"))
            {
                onStunBatonPickup?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}