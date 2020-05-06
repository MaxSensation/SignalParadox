using System;
using System.Linq;
using Interactables.Button;
using UnityEngine;

public class DisableAllBoxColliders : MonoBehaviour
{
    private void Awake()
    {
        ButtonController.onButtonPressed += Disable;
    }

    private void OnDestroy()
    {
        ButtonController.onButtonPressed -= Disable;
    }

    private void Disable(GameObject[] interactable)
    {
        if (interactable.Contains(gameObject))
        {
            foreach (var t in transform.GetComponentsInChildren<Transform>())
            {
                foreach (var b in t.GetComponentsInChildren<Transform>())
                {
                    if (b.name == "Trigger")
                    {
                        b.GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
        }
    }
}
