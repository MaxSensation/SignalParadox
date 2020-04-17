using EventSystem;
using UnityEngine;
using EventHandler = EventSystem.EventHandler;

public class TriggerCheckPoint : MonoBehaviour
{
    private bool _checkPointUsed;
    private void OnTriggerEnter(Collider other)
    {
        if (!_checkPointUsed && other.CompareTag("Player"))
        {
            Debug.Log("CheckpointTrigger activated");
            EventHandler.InvokeEvent(new OnTriggerEnteredCheckPointEvent());
            _checkPointUsed = true;
        }
    }
}
