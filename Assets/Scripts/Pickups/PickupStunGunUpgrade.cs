using EventSystem;
using UnityEngine;
using EventHandler = EventSystem.EventHandler;

namespace Pickups
{
    public class PickupStunGunUpgrade : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other && other.CompareTag("Player"))
            {
                Debug.Log("Found StunGunUpgrade");
                EventHandler.InvokeEvent(new OnPickupStunGunUpgradeEvent());
                Destroy(gameObject);
            }
        }
    }
}
