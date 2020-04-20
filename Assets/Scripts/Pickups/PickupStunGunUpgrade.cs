using UnityEngine;

namespace Pickups
{
    public class PickupStunGunUpgrade : MonoBehaviour
    {
        public delegate void OnStunGunUpgradePickup();

        public static event OnStunGunUpgradePickup onStunGunUpgradePickup;
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
