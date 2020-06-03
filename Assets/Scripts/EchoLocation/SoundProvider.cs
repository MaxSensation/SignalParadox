//Main author: Maximiliam Rosén

using System.Collections;
using UnityEngine;

namespace EchoLocation
{
    public class SoundProvider : MonoBehaviour
    {
        [SerializeField] [Range(1,1000)] private int xResolution, yResolution, maxBounces;
        [SerializeField] [Range(0f,10f)] private float frequency;
        [SerializeField] [Range(0f,1f)] private float airDecay, minStrengthTolerance, bouncesDecay;
        [SerializeField] private LayerMask bounceLayer;

        private EchoLocationTransmitter transmitter;
        private WaitForSeconds frequencySecounds;
        private Coroutine echo;
        private void Awake()
        {
            frequencySecounds = new WaitForSeconds(frequency);
            transmitter = GetComponent<EchoLocationTransmitter>();
        }

        private void OnEnable() => echo = StartCoroutine(Echo());
        private void OnDisable() => StopCoroutine(echo);
        public void SetSoundStrength(float tolerance) => minStrengthTolerance = 1 - tolerance;

        private IEnumerator Echo()
        {
            while (true)
            {
                yield return frequencySecounds;
                transmitter.CastAudioRays(xResolution, yResolution, maxBounces, bouncesDecay, airDecay, minStrengthTolerance, bounceLayer);            
            }
        }
    }
}