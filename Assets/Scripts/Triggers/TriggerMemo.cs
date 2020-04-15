using EventSystem;
using UnityEngine;
using EventHandler = EventSystem.EventHandler;

namespace Triggers
{
    public class TriggerMemo : MonoBehaviour
    {
        [SerializeField] private AudioClip memo;
        private bool hasPlayed;
        private void OnTriggerEnter(Collider other)
        {
            if (!hasPlayed && other.CompareTag("Player"))
            {
                Debug.Log("Memo Found");
                hasPlayed = true;
                EventHandler.InvokeEvent(new OnTriggerMemoEvent(memo));
            }
        }
    }
}
