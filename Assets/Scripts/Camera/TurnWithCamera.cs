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
        var _transform = transform;
        objectTransform = _transform;
        parentTransform = _transform.parent;
    }
    
    private void LateUpdate () {
        if (!Active) return;
        if(!cameraTransform) return;
        var _up = parentTransform.up;
        var _forwardDirection = Vector3.ProjectOnPlane(cameraTransform.forward, _up).normalized;
        var _upDirection = _up;
        objectTransform.rotation = Quaternion.LookRotation(_forwardDirection, _upDirection);
    }
}
