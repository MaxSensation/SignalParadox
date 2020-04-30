using System;
using UnityEngine;

public class SoundListener : MonoBehaviour
{
    private Vector3 lastKnownLocation;
    public Action<Vector3> onHeardSound;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SoundEmitter"))
        {
            onHeardSound?.Invoke(other.transform.position);
        }
    }
}
