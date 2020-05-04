//Main author: Maximiliam Rosén

using UnityEngine;

namespace EchoLocation
{
    public class EchoLocationResult
    {
        public float BestSoundStrength;
        public float BestSoundDistance;
        public Vector3 LastBounceLocation;
        public GameObject Transmitter;
        public GameObject Receiver;

        public EchoLocationResult(float bestSoundStrength, float bestSoundDistance, Vector3 lastBounceLocation, GameObject transmitter, GameObject receiver)
        {
            BestSoundStrength = bestSoundStrength;
            BestSoundDistance = bestSoundDistance;
            LastBounceLocation = lastBounceLocation;
            Transmitter = transmitter;
            Receiver = receiver;
        }
    }
}
