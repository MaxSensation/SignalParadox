using UnityEngine;

namespace Pickups
{
    public class PickupStunBaton : MonoBehaviour
    {
        public delegate void OnStunBatonPickup();

        public static event OnStunBatonPickup onStunBatonPickup;
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