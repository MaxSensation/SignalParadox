//Main author: Andreas Berzelius

using System;
using Player;
using UnityEngine;

namespace DecoyGrenade
{
    public class PickupDecoyGrenade : MonoBehaviour
    {
        [SerializeField] private bool shouldDespawnOnPickup = true;
        private AudioSource audioSource;
        private bool hasGrenade;

        public static Action onGrenadePickupEvent;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            PlayerAnimatorController.onTouchedGrenade += Pickup;
            ThrowDecoyGrenade.onPickedUpGrenadeEvent += () => hasGrenade = true;;
            ThrowDecoyGrenade.onThrowEvent += () => hasGrenade = false;
        }
        
        private void OnDestroy()
        {
            PlayerAnimatorController.onTouchedGrenade -= Pickup;
            ThrowDecoyGrenade.onPickedUpGrenadeEvent -= () => hasGrenade = true;;
            ThrowDecoyGrenade.onThrowEvent -= () => hasGrenade = false;
        }

        private void Pickup(GameObject obj)
        {
            if (gameObject != obj || hasGrenade) return;
            audioSource.Play();
            onGrenadePickupEvent?.Invoke();
            gameObject.SetActive(!shouldDespawnOnPickup);
        }
    }
}
