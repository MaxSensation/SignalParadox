using System;
using UnityEngine;

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
