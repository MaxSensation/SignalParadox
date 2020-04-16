using EventSystem;
using UnityEngine;
using EventHandler = EventSystem.EventHandler;

public class PlayerInteractTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Button"))
            EventHandler.InvokeEvent(new OnPlayerEnteredInteractionEvent(other.gameObject));
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Button"))
            EventHandler.InvokeEvent(new OnPlayerExitedInteractionEvent(other.gameObject));
    }
}
