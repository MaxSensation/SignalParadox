using UnityEngine;

public class TriggerCheckPoint : MonoBehaviour
{
    private bool _checkPointUsed;
    public delegate void OnTriggerCheckPoint(CheckPoint checkPoint);
    public static event OnTriggerCheckPoint onTriggerCheckPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (!_checkPointUsed && other.CompareTag("Player"))
        {
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
