using System;
using UnityEngine;

namespace Triggers
{
    public class TriggerMemo : MonoBehaviour
    {
        [SerializeField] private AudioClip memo;
        private bool hasPlayed;

        public static Action<AudioClip> onMemoPickup;
        private void OnTriggerEnter(Collider other)
        {
            if (!hasPlayed && other.CompareTag("Player"))
            {
                Debug.Log("Memo Found");
                hasPlayed = true;
                onMemoPickup?.Invoke(memo);
            }
        }
    }
}
