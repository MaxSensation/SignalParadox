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
                Debug.Log("Found StunGunUpgrade");
                onStunGunUpgradePickup?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
