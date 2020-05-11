using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHeadWithCamera : MonoBehaviour
{
    [Tooltip("The max and min angle from the default state")][SerializeField] private float maxAngle;
    private Transform cameraTransform;
    private Transform objectTransform;
    private Transform parentTransform;

    private void Start ()
    {
        if (Camera.main != null) cameraTransform = Camera.main.transform;
        objectTransform = transform;
        parentTransform = objectTransform.parent;
    }

    private void LateUpdate() {
        if(!cameraTransform) return;
        var newLookXRotation = Quaternion.LookRotation(cameraTransform.forward, parentTransform.up);
        var currentLocalEulerAngles = objectTransform.localEulerAngles;
        var wantedLocalEulerXAngle = ClampAngle(newLookXRotation.eulerAngles.x, -maxAngle, maxAngle);
        var wantedLocalEulerAngles = new Vector3(wantedLocalEulerXAngle, currentLocalEulerAngles.y, currentLocalEulerAngles.z);
        objectTransform.localEulerAngles = wantedLocalEulerAngles;
        // var newRotation = Quaternion.Euler(wantedLocalEulerAngles);
        // objectTransform.localRotation = Quaternion.Lerp(objectTransform.localRotation, newRotation, Time.deltaTime * smoothSpeed);
    }
    
    private static float ClampAngle(float eulerAngle, float min, float max)
    {
        if (eulerAngle < 0f) eulerAngle = 360 + eulerAngle;
        return eulerAngle > 180f ? Mathf.Max(eulerAngle, 360 + min) : Mathf.Min(eulerAngle, max);
    }
}
