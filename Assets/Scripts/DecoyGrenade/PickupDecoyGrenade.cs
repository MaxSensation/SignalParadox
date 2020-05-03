using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickupDecoyGrenade : MonoBehaviour
{
    [SerializeField] private int _grenadeAmount;

    public static Action<int> onGrenadePickup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Picked up Decoygrenade");
            onGrenadePickup?.Invoke(_grenadeAmount);
            gameObject.SetActive(false);
        }
    }
}
