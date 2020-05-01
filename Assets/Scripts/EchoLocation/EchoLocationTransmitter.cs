using UnityEngine;

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
        private float _bestSoundStrength;
        private float _bestSoundDistance;
        private Vector3 _lastBounceLocation;
        private GameObject _receiver;
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

        public EchoLocationResult CreateTestSound()
        {
            return CastAudioRays(debugXResolution, debugYResolution, debugMaxBounces, debugBouncesDecay, debugAirDecay, debugMinStrengthTolerance, debugBounceLayer);
        }

        public EchoLocationResult CastAudioRays(int xResolution, int yResolution, int maxBounces, float bouncesDecay, float airDecay, float minStrengthTolerance, LayerMask bounceLayer)
        {
            Gizmos.color = Color.red;
            _bestSoundStrength = 0f;
            _bestSoundDistance = 0f;
            _receiver = null;
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
        
            if (_receiver == null) return null;
            var echoLocationResult = new EchoLocationResult(_bestSoundStrength, _bestSoundDistance, _lastBounceLocation, gameObject, _receiver);
            _receiver.GetComponent<EchoLocationReceiver>().ReceiveHit(echoLocationResult);
            return echoLocationResult;
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
                    if (_bestSoundStrength < strength)
                    {
                        _lastBounceLocation = lastBounceLocation;
                        _bestSoundStrength = strength;
                        _bestSoundDistance = distance;
                        _receiver = hit.collider.gameObject;
                    }
                    break;
                }
                strength *= 1 - bouncesDecay;
            }
        }
    }
}
