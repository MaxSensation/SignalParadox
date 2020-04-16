﻿using EventSystem;
using UnityEngine;
using EventHandler = EventSystem.EventHandler;

namespace Pickups
{
    public class PickupStunBaton : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other && other.CompareTag("Player"))
            {
                Debug.Log("Found StunBaton");
                EventHandler.InvokeEvent(new OnPickupStunBatonEvent());
                Destroy(gameObject);
            }
        }
    }
}