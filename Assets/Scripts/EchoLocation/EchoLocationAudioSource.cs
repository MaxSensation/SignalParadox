using UnityEngine;

namespace EchoLocation
{
    public class EchoLocationAudioSource : MonoBehaviour
    {
        [SerializeField] [Range(0f,1f)] private float audioSourceTransitionSpeed;
        private AudioSource _audioSource;
        private EchoLocationTransmitter _echoLocationTransmitter;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _echoLocationTransmitter = transform.GetComponentInParent<EchoLocationTransmitter>();
        }

        private void Update()
        {
            var echoLocationResult = _echoLocationTransmitter.CreateTestSound();
            if (echoLocationResult != null)
            {
                _audioSource.volume = echoLocationResult.BestSoundStrength;
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, echoLocationResult.LastBounceLocation, Time.deltaTime * audioSourceTransitionSpeed);            
            }
            else
            {
                _audioSource.volume = 0.1f;
            }
        }
    }
}
