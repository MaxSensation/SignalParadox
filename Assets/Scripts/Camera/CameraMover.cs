using UnityEngine;

public class CameraMover : MonoBehaviour
{
	[SerializeField] private LayerMask layerMask = ~0;
	[SerializeField] private float smoothingFactor = 25f;
	[SerializeField] private float spherecastRadius = 0.2f;
	private Transform cameraTransform, objectTransform;
	private float preferredDistance, currentDistance;
	private Vector3 localCastDirection;
	private void Awake () {
		objectTransform = transform;
		if (Camera.main != null) cameraTransform = Camera.main.transform;
		var _cameraTransformPosition = cameraTransform.position;
		var _objectTransformPosition = objectTransform.position;
		preferredDistance = (_cameraTransformPosition - _objectTransformPosition).magnitude;
		currentDistance = preferredDistance;
		localCastDirection = _cameraTransformPosition - _objectTransformPosition;
		localCastDirection.Normalize();
		localCastDirection = objectTransform.worldToLocalMatrix * localCastDirection;
	}

	private void LateUpdate () {
		var _distance = GetCameraDistance();
		currentDistance = Mathf.Lerp(currentDistance, _distance, Time.deltaTime * smoothingFactor);
		Vector3 _direction = objectTransform.localToWorldMatrix * localCastDirection;
		cameraTransform.position = objectTransform.position + _direction * currentDistance;
	}

	private float GetCameraDistance()
	{
		Vector3 _direction = objectTransform.localToWorldMatrix * localCastDirection;
		return Physics.SphereCast(new Ray(objectTransform.position, _direction), spherecastRadius, out var _hit, preferredDistance, layerMask, QueryTriggerInteraction.Ignore) ? _hit.distance : preferredDistance;
	}
}
