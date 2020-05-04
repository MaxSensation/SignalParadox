//Main author: Maximiliam Rosén

using System;
using Interactables.CheckPointSystem;
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
            var checkPoint = CheckPointGenerator.Generate();
            if (checkPoint != null)
            {
                onTriggerCheckPoint?.Invoke(checkPoint);
                _checkPointUsed = true;
            }
        }
    }
}
