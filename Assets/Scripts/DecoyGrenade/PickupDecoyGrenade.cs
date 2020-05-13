//Main author: Andreas Berzelius

using UnityEngine;
using System;

public class PickupDecoyGrenade : MonoBehaviour
{
    [SerializeField] private int _grenadeAmount = 1;
    [SerializeField] private bool _shouldDespawnOnPickup = true;
    private AudioSource audioSource;

    public static Action<int> onGrenadePickup;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ThrowDecoyGrenade.OnPickedUpGrenade += Pickup;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onGrenadePickup?.Invoke(_grenadeAmount);
            gameObject.SetActive(!_shouldDespawnOnPickup);
        }
    }

    private void OnDestroy()
    {
        ThrowDecoyGrenade.OnPickedUpGrenade -= Pickup;
    }

    private void Pickup()
    {
        audioSource.Play();
    }
}
