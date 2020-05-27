//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace Interactables.Triggers.Memo
{
    public class TriggerMemo : MonoBehaviour
    {
        [SerializeField] private AudioClip memo;
        private bool hasPlayed;
        private AudioSource audioSource;
        public static Action<AudioClip> onMemoPickupEvent;

        private void Start() => audioSource = GetComponent<AudioSource>();
        
        private void OnTriggerEnter(Collider other)
        {
            if (hasPlayed || !other.CompareTag("Player")) return;
            Debug.Log("Memo Found");
            hasPlayed = true;
            onMemoPickupEvent?.Invoke(memo);
            audioSource.Stop();
        }
    }
}