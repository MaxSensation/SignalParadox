//Main author: Maximiliam Rosén

using UnityEngine;

namespace EchoLocation
{
    public struct EchoLocationResult
    {
        public float BestSoundStrength, BestSoundDistance;
        public GameObject Transmitter, Receiver;
        public Vector3 LastBounceLocation;

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