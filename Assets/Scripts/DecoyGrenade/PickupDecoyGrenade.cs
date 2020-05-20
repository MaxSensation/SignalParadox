//Main author: Andreas Berzelius

using UnityEngine;
using System;
using Player;

public class PickupDecoyGrenade : MonoBehaviour
{
    [SerializeField] private bool _shouldDespawnOnPickup = true;
    private AudioSource audioSource;
    private bool hasGrenade;

    public static Action onGrenadePickup;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerInteractionTrigger.onInteractedEvent += Pickup;
        ThrowDecoyGrenade.OnPickedUpGrenade += PickedUpGrenade;
        ThrowDecoyGrenade.OnThrowEvent += ThrownGrenade;
    }

    private void ThrownGrenade()
    {
        hasGrenade = false;
    }

    private void PickedUpGrenade()
    {
        hasGrenade = true;
    }

    private void Pickup(GameObject obj)
    {
        if (gameObject == obj && !hasGrenade)
        {
            audioSource.Play();
            onGrenadePickup?.Invoke();
            gameObject.SetActive(!_shouldDespawnOnPickup);
        }
    }

    private void OnDestroy()
    {
        PlayerInteractionTrigger.onInteractedEvent -= Pickup;
        ThrowDecoyGrenade.OnPickedUpGrenade -= PickedUpGrenade;
        ThrowDecoyGrenade.OnThrowEvent -= ThrownGrenade;
    }
}
