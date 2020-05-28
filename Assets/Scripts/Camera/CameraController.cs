//Main author: Maximiliam Rosén

using Player.PlayerStateMachine;
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
	private InputCamera inputCamera;

	private float crouchCameraPosition;
	private float crouchSmoothTime;
	private bool isCrouching;

	private void Awake () {
		objectTransform = transform;
		inputCamera = GetComponent<InputCamera>();
		if(cam == null)
			cam = GetComponentInChildren<Camera>();
		var localRotation = objectTransform.localRotation;
		currentXAngle = localRotation.eulerAngles.x;
		currentYAngle = localRotation.eulerAngles.y;
		RotateCamera(0f, 0f);
		crouchCameraPosition = -0.5f;
		crouchSmoothTime = 10f;
		
		//Events
		CrouchState.onEnteredCrouchEvent += EnterCrouchCameraPosition;
		CrouchState.onExitCrouchEvent += ExitCrouchCameraPosition;
	}

	private void OnDestroy()
	{
		CrouchState.onEnteredCrouchEvent -= EnterCrouchCameraPosition;
		CrouchState.onExitCrouchEvent -= ExitCrouchCameraPosition;
	}

	private void EnterCrouchCameraPosition() => isCrouching = true;
	
	private void ExitCrouchCameraPosition() => isCrouching = false;

	private void Update()
	{
		HandleCameraRotation();
		HandleCrouch();
	}

	private void HandleCrouch()
	{
		if (isCrouching)
			objectTransform.transform.localPosition = 
				new Vector3(0,Mathf.Lerp(objectTransform.transform.localPosition.y, crouchCameraPosition, Time.deltaTime * crouchSmoothTime),0);
		else
			objectTransform.transform.localPosition = 
				new Vector3(0,Mathf.Lerp(objectTransform.transform.localPosition.y, 0, Time.deltaTime * crouchSmoothTime),0);
	}

	private void HandleCameraRotation()
	{
		var inputHorizontal = inputCamera.GetHorizontalCameraInput();
		var inputVertical = inputCamera.GetVerticalCameraInput();
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
