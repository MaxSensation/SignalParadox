//Main author: Maximiliam Rosén

using System;
using Managers;
using SaveSystem;
using UnityEngine;

namespace Interactables.Triggers
{
    public class TriggerCheckPoint : MonoBehaviour
    {
        private bool _checkPointUsed;
        public static Action<CheckPoint> onTriggerCheckPoint;
        private void OnTriggerEnter(Collider other)
        {
            if (_checkPointUsed || !other.CompareTag("Player")) return;
            Debug.Log("CheckpointTrigger activated");
            onTriggerCheckPoint?.Invoke(new CheckPoint(transform));
            _checkPointUsed = true;
        }
    }
}
