//Main author: Maximiliam Rosén

using UnityEngine;

namespace TransformModifier
{
	public class RotationSmooth : MonoBehaviour
	{
		[SerializeField] private float smoothSpeed = 20f;
		private Transform target;
		private Transform objectTransform;
		private Quaternion currentRotation;
	
		private void Awake () {
			var thisTransform = transform;
			target = thisTransform.parent;
			objectTransform = thisTransform;
			currentRotation = thisTransform.rotation;
		}
		
		private void LateUpdate () => objectTransform.rotation = Smooth();
		private Quaternion Smooth() => Quaternion.Slerp(currentRotation, target.rotation, Time.deltaTime * smoothSpeed);
	}
}
