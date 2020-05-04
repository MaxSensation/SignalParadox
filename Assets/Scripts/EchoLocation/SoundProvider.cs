//Main author: Maximiliam Rosén

using System.Collections;
using UnityEngine;

namespace EchoLocation
{
    public class SoundProvider : MonoBehaviour
    {
        [SerializeField] [Range(0f,10f)] private float frequency;
        [SerializeField] [Range(1,1000)] private int xResolution;
        [SerializeField] [Range(1,1000)] private int yResolution;
        [SerializeField] [Range(1,1000)] private int maxBounces;
        [SerializeField] [Range(0f,1f)] private float bouncesDecay;
        [SerializeField] [Range(0f,1f)] private float airDecay;
        [SerializeField] [Range(0f,1f)] private float minStrengthTolerance;
        [SerializeField] private LayerMask bounceLayer;

        private EchoLocationTransmitter _transmitter;
        private Coroutine echo;
        private void Awake()
        {
            _transmitter = GetComponent<EchoLocationTransmitter>();
        }

        private void OnEnable()
        {
            echo = StartCoroutine("Echo");
        }

        private void OnDisable()
        {
            StopCoroutine(echo);
        }

        private IEnumerator Echo()
        {
            while (true)
            {
                yield return new WaitForSeconds(frequency);
                _transmitter.CastAudioRays(xResolution, yResolution, maxBounces, bouncesDecay, airDecay, minStrengthTolerance, bounceLayer);            
            }
        }

        public void SetMinStrengthTolerance(float tolerance)
        {
            minStrengthTolerance = tolerance;
        }

        public void SetSoundStrength(float tolerance)
        {
            minStrengthTolerance = tolerance;
        }
    }
}
