//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace Interactables.Triggers
{
    public class TriggerMemo : MonoBehaviour
    {
        [SerializeField] private AudioClip memo;
        private bool hasPlayed;
        private AudioSource ringingSound;
        public static Action<AudioClip> onMemoPickupEvent;

        private void Start() => ringingSound = GetComponent<AudioSource>();

        private void OnTriggerEnter(Collider other)
        {
            if (hasPlayed || !other.CompareTag("Player")) return;
            hasPlayed = true;
            onMemoPickupEvent?.Invoke(memo);
            ringingSound.Stop();
        }
    }
}
