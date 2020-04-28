using UnityEngine;

public class RotationSmooth : MonoBehaviour
{
	[SerializeField] private float smoothSpeed = 20f;
	private Transform target;
	private Transform objectTransform;
	private Quaternion currentRotation;
	
	private void Awake () {
		target = transform.parent;
		var _transform = transform;
		objectTransform = _transform;
		currentRotation = _transform.rotation;
	}

	private void LateUpdate () {
		SmoothUpdate();
	}

	private void SmoothUpdate()
	{
		currentRotation = Smooth (currentRotation, target.rotation, smoothSpeed);
		objectTransform.rotation = currentRotation;
	}

	private Quaternion Smooth(Quaternion _currentRotation, Quaternion _targetRotation, float _smoothSpeed)
	{
		return Quaternion.Slerp (_currentRotation, _targetRotation, Time.deltaTime * _smoothSpeed);
	}
}
