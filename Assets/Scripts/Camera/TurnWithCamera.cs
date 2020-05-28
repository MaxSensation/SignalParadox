//Main author: Maximiliam Rosén

using UnityEngine;

public class TurnWithCamera : MonoBehaviour
{
    internal static bool Active { get; set; }
    private Transform cameraTransform;
    private Transform objectTransform;
    private Transform parentTransform;

    private void Start ()
    {
        Active = true;
        if (Camera.main != null) cameraTransform = Camera.main.transform;
        objectTransform = transform;
        parentTransform = objectTransform.parent;
    }
    
    private void LateUpdate () {
        if (!Active) return;
        if(!cameraTransform) return;
        var up = parentTransform.up;
        var forwardDirection = Vector3.ProjectOnPlane(cameraTransform.forward, up).normalized;
        var upDirection = up;
        objectTransform.rotation = Quaternion.LookRotation(forwardDirection, upDirection);
    }
}
