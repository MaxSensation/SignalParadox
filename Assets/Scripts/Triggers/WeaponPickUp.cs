using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other && other.CompareTag("Player"))
        {
            Debug.Log("Found StunGun");
            other.GetComponent<PlayerController>().EnableStunGun();
            Destroy(gameObject);
        }
    }
}
