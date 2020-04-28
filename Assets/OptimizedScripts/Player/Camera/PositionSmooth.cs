using UnityEngine;

public class PositionSmooth : MonoBehaviour {
	[SerializeField] private float lerpSpeed = 20f;
	[SerializeField] private float smoothDampTime = 0.02f;
	private Transform target;
	private Transform objectTransform;
	private Vector3 currentPosition;
	private Vector3 localPositionOffset;
	private Vector3 refVelocity;
	
	public void Awake () {
		var _transform = transform;
		target = _transform.parent;
		objectTransform = _transform;
		currentPosition = _transform.position;
		localPositionOffset = objectTransform.localPosition;
	}

	private void LateUpdate () {
		SmoothUpdate();
	}

	private void SmoothUpdate()
	{
		currentPosition = SmoothPosition (currentPosition, target.position, lerpSpeed);
		objectTransform.position = currentPosition;
	}

	private Vector3 SmoothPosition(Vector3 _start, Vector3 _target, float _smoothTime)
	{
		Vector3 _offset = objectTransform.localToWorldMatrix * localPositionOffset;
		_target += _offset;
		return Vector3.SmoothDamp (_start, _target, ref refVelocity, smoothDampTime);
	}
}