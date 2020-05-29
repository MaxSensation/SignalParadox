//Main author: Maximiliam Rosén

using UnityEngine;

namespace TransformModifier
{
	public class PositionSmooth : MonoBehaviour {
		[SerializeField] private float smoothDampTime = 0.02f;
		private Transform target;
		private Transform objectTransform;
		private Vector3 currentPosition;
		private Vector3 localPositionOffset;
		private Vector3 refVelocity;
	
		public void Awake () {
			objectTransform = transform;
			target = objectTransform.parent;
			currentPosition = objectTransform.position;
			localPositionOffset = objectTransform.localPosition;
		}
		
		private void LateUpdate () => SmoothUpdate();

		private void SmoothUpdate()
		{
			currentPosition = SmoothPosition(currentPosition, target.position, smoothDampTime);
			objectTransform.position = currentPosition;
		}

		private Vector3 SmoothPosition(Vector3 startPos, Vector3 targetPos, float smoothTime)
		{
			targetPos += (Vector3)(objectTransform.localToWorldMatrix * localPositionOffset);
			return Vector3.SmoothDamp (startPos, targetPos, ref refVelocity, smoothTime);
		}
	}
}