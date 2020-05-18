using System;
using UnityEngine;

public class ResetCamera : MonoBehaviour
{
    [SerializeField] private Vector3 _cameraValues;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void Reset()
    {
        var cameraTransform = _camera.transform;
        cameraTransform.localPosition = _cameraValues;
        cameraTransform.rotation = new Quaternion();
    }
}
