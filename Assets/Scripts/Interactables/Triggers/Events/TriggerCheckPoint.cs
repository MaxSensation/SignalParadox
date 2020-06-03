//Main author: Maximiliam Rosén

using System;
using SaveSystem;
using UnityEngine;

namespace Interactables.Triggers.Events
{
    public class TriggerCheckPoint : MonoBehaviour
    {
        private bool checkPointUsed;
        public static Action<CheckPoint> onTriggerCheckPoint;
        private void OnTriggerEnter(Collider other)
        {
            if (checkPointUsed || !other.CompareTag("Player")) return;
            Debug.Log("CheckpointTrigger activated");
            onTriggerCheckPoint?.Invoke(new CheckPoint(other.gameObject, transform));
            checkPointUsed = true;
        }
    }
}
