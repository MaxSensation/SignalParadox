using UnityEngine;

namespace EventSystem
{
    public class SoundListener : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnDestroy()
        {
        }
    }
}
