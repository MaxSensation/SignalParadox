﻿using UnityEngine;

namespace EchoLocation
{
    public class EchoLocationTransmitter : MonoBehaviour
    {
        [SerializeField] private bool debugModeOn;
        [SerializeField] private bool debugTestSoundOn;
        [SerializeField] [Range(1,1000)] private int debugXResolution;
        [SerializeField] [Range(1,1000)] private int debugYResolution;
        [SerializeField] [Range(1,1000)] private int debugMaxBounces;
        [SerializeField] [Range(0f,1f)] private float debugBouncesDecay;
        [SerializeField] [Range(0f,1f)] private float debugAirDecay;
        [SerializeField] [Range(0f,1f)] private float debugMinStrengthTolerance;
        [SerializeField] private LayerMask debugBounceLayer;
        private Transform _tm;

        private void Update()
        {
            if (debugTestSoundOn)
                CreateTestSound();
        }

        public void SetSoundStrength(float tolerance)
        {
            debugMinStrengthTolerance = tolerance;
        }

        public void CreateTestSound()
        {
            CastAudioRays(debugXResolution, debugYResolution, debugMaxBounces, debugBouncesDecay, debugAirDecay, debugMinStrengthTolerance, debugBounceLayer);
        }

        public void CastAudioRays(int xResolution, int yResolution, int maxBounces, float bouncesDecay, float airDecay, float minStrengthTolerance, LayerMask bounceLayer)
        {
            Gizmos.color = Color.red;
            _tm = transform;
            var direction = transform.forward;
            var directionChangeAmount = 360 / xResolution; 
            for (var x = 0; x < xResolution; x++)
            {
                direction = Quaternion.Euler(directionChangeAmount, 0, directionChangeAmount) * direction;
                directionChangeAmount = 360 / yResolution; 
                for (var y = 0; y < yResolution; y++)
                {
                    CastAudioRay(direction, maxBounces, bouncesDecay, airDecay, minStrengthTolerance, bounceLayer);
                    direction = Quaternion.Euler(0, directionChangeAmount, 0) * direction;
                }
            }
        }

        private void CastAudioRay(Vector3 direction, int maxBounces, float bouncesDecay, float airDecay, float minStrengthTolerance, LayerMask bounceLayer)
        {
            var location = _tm.position;
            var distance = 0f;
            var strength = 1f;
            for (var i = 0; i < maxBounces; i++)
            {
                Physics.Raycast(location, direction, out var hit, float.PositiveInfinity, bounceLayer);
                if (debugModeOn)
                    Debug.DrawLine(location, hit.point);
                var lastBounceLocation = location;
                location = hit.point;
                direction = Vector3.Reflect(direction, hit.normal);
                distance += hit.distance;
                strength *= 1f - distance * airDecay;
                if(strength < minStrengthTolerance)
                    break;
                if (hit.collider != null && hit.collider.CompareTag("Receiver"))
                {
                    var echoLocationResult = new EchoLocationResult(strength, distance, lastBounceLocation, gameObject, hit.collider.gameObject);
                    hit.collider.GetComponent<EchoLocationReceiver>().ReceiveHit(echoLocationResult);
                }
                strength *= 1 - bouncesDecay;
            }
        }
    }
}