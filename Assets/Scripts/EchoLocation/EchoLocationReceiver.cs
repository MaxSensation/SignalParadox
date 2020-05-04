//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace EchoLocation
{
    public class EchoLocationReceiver : MonoBehaviour
    {
        public Action<EchoLocationResult> heardSound;
        public void ReceiveHit(EchoLocationResult echoLocationResult)
        {
            heardSound?.Invoke(echoLocationResult);
            //Debug.Log("Receiver: " + gameObject.name + " was hit from Transmitter: " + echoLocationResult.Transmitter.name + "From a distance at: " + echoLocationResult.BestSoundDistance + " meters with a sound strength of " + echoLocationResult.BestSoundStrength * 100 + "%");
        }
    }
}
