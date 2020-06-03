//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace EchoLocation
{
    public class EchoLocationReceiver : MonoBehaviour
    {
        public Action<EchoLocationResult> heardSound;
        public void ReceiveHit(EchoLocationResult echoLocationResult) => heardSound?.Invoke(echoLocationResult);
    }
}