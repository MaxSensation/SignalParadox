//Main author: Andreas Berzelius

using UnityEngine;
using System;

public class PickupDecoyGrenade : MonoBehaviour
{
    [SerializeField] private int _grenadeAmount = 1;
    [SerializeField] private bool _shouldDespawnOnPickup = true;

    public static Action<int> onGrenadePickup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Picked up Decoygrenade");
            onGrenadePickup?.Invoke(_grenadeAmount);
            gameObject.SetActive(!_shouldDespawnOnPickup);
        }
    }
}
