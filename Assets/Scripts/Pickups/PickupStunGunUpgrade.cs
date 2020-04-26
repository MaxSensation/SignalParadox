//TODO Create one Class that does the below and the pickupStunGun

using System;
using UnityEngine;

namespace Pickups
{
    public class PickupStunGunUpgrade : MonoBehaviour
    {
        public static Action onStunGunUpgradePickup;
        private void OnTriggerEnter(Collider other)
        {
            if (other && other.CompareTag("Player"))
            {
                onStunGunUpgradePickup?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
