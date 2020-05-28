//Main author: Maximiliam Rosén

using UnityEngine;

public class TurnHeadWithCamera : MonoBehaviour
{
    [Tooltip("The max and min angle from the default state")][SerializeField] private float maxAngle;
    private Transform cameraTransform, headTransform, parentTransform;

    private void Awake ()
    {
        var camera = Camera.main;
        if (camera != null) cameraTransform = camera.transform;
        headTransform = transform;
        parentTransform = headTransform.parent;
    }

    private void LateUpdate() {
        if(!cameraTransform) return;
        var newLookXRotation = Quaternion.LookRotation(cameraTransform.forward, parentTransform.up);
        var currentLocalEulerAngles = headTransform.localEulerAngles;
        var wantedLocalEulerXAngle = ClampAngle(newLookXRotation.eulerAngles.x, -maxAngle, maxAngle);
        var wantedLocalEulerAngles = new Vector3(wantedLocalEulerXAngle, currentLocalEulerAngles.y, currentLocalEulerAngles.z);
        headTransform.localEulerAngles = wantedLocalEulerAngles;
    }
    
    private static float ClampAngle(float eulerAngle, float min, float max)
    {
        if (eulerAngle < 0f) eulerAngle = 360 + eulerAngle;
        return eulerAngle > 180f ? Mathf.Max(eulerAngle, 360 + min) : Mathf.Min(eulerAngle, max);
    }
}
