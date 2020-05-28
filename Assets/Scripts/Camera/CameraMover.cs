//Main author: Maximiliam Rosén

using UnityEngine;

public class CameraMover : MonoBehaviour
{
	[SerializeField] private LayerMask layerMask = ~0;
	[SerializeField] private float smoothingFactor = 25f;
	[SerializeField] private float spherecastRadius = 0.2f;
	private Transform cameraTransform, cameraContainerTransform;
	private float preferredDistance, currentDistance;
	private Vector3 localCastDirection;

	private void Awake () {
		cameraContainerTransform = transform;
        var camera = Camera.main;
        if (camera != null) cameraTransform = camera.transform;
		var cameraTransformPosition = cameraTransform.position;
		var objectTransformPosition = cameraContainerTransform.position;
		preferredDistance = (cameraTransformPosition - objectTransformPosition).magnitude;
		currentDistance = preferredDistance;
		localCastDirection = cameraTransformPosition - objectTransformPosition;
		localCastDirection.Normalize();
		localCastDirection = cameraContainerTransform.worldToLocalMatrix * localCastDirection;
	}

	private void LateUpdate () {
		var distance = GetCameraDistance();
		currentDistance = Mathf.Lerp(currentDistance, distance, Time.deltaTime * smoothingFactor);
		Vector3 direction = cameraContainerTransform.localToWorldMatrix * localCastDirection;
		cameraTransform.position = cameraContainerTransform.position + direction * currentDistance;
	}

	private float GetCameraDistance()
	{
		Vector3 direction = cameraContainerTransform.localToWorldMatrix * localCastDirection;
		return Physics.SphereCast(new Ray(cameraContainerTransform.position, direction), spherecastRadius, out var hit, preferredDistance, layerMask, QueryTriggerInteraction.Ignore) ? hit.distance : preferredDistance;
	}
}
