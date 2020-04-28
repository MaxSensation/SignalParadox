using UnityEngine;

public sealed class CameraController : MonoBehaviour
{
	[SerializeField] [Range(0f, 90f)] private float upVerticalLimit = 60f;
	[SerializeField] [Range(0f, 90f)] private float downVerticalLimit = 60f;
	[SerializeField] private float cameraTurningSpeed = 50f;

	private float currentXAngle, currentYAngle;
	private float oldHorizontalInput, oldVerticalInput;

	private Transform objectTransform;
	private Camera cam;
	private MouseInputCamera mouseInputCamera;

	private void Awake () {
		objectTransform = transform;
		mouseInputCamera = GetComponent<MouseInputCamera>();
		if(cam == null)
			cam = GetComponentInChildren<Camera>();
		var _localRotation = objectTransform.localRotation;
		currentXAngle = _localRotation.eulerAngles.x;
		currentYAngle = _localRotation.eulerAngles.y;
		RotateCamera(0f, 0f);
	}

	private void Update()
	{
		HandleCameraRotation();
		HandleCrouch();
	}

	private void HandleCrouch()
	{
		if (mouseInputCamera.IsCrouching())
			objectTransform.transform.localPosition = new Vector3(0, -0.5f, 0);
		else
			objectTransform.transform.localPosition = new Vector3(0, 0, 0);
	}

	private void HandleCameraRotation()
	{
		var inputHorizontal = mouseInputCamera.GetHorizontalCameraInput();
		var inputVertical = mouseInputCamera.GetVerticalCameraInput();
		inputHorizontal *= Time.deltaTime * cameraTurningSpeed;
		inputVertical *= Time.deltaTime * cameraTurningSpeed;
		RotateCamera(inputHorizontal, inputVertical);
	}
	
	private void RotateCamera(float _newHorizontalInput, float _newVerticalInput)
	{
		oldHorizontalInput = _newHorizontalInput;
		oldVerticalInput = _newVerticalInput;
		currentXAngle += oldVerticalInput;
		currentYAngle += oldHorizontalInput;
		currentXAngle = Mathf.Clamp(currentXAngle, -upVerticalLimit, downVerticalLimit);
		UpdateRotation();
	}
	
	private void UpdateRotation()
	{
		objectTransform.localRotation = Quaternion.Euler(new Vector3(0, currentYAngle, 0));
		objectTransform.localRotation = Quaternion.Euler(new Vector3(currentXAngle, currentYAngle, 0));
	}
}
