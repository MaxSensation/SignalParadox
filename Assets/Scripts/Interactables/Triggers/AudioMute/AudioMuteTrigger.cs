//Main author: Maximiliam Rosén

using UnityEngine;

namespace Interactables.Triggers.AudioMute
{
    public class AudioMuteTrigger : MonoBehaviour
    {
        [SerializeField] private AudioSource[] audioSources;
        private bool isTriggerd;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || isTriggerd) return;
            DeactivateAudioSoruces();
            isTriggerd = true;
        }

        private void DeactivateAudioSoruces()
        {
            for (var i = 0; i < audioSources.Length; i++)
                audioSources[i].enabled = false;
        }
    }
}
