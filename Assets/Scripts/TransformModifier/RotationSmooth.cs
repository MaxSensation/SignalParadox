//Main author: Maximiliam Rosén

using UnityEngine;

namespace TransformModifier
{
    public class RotationSmooth : MonoBehaviour
    {
        [SerializeField] private float smoothSpeed = 20f;
        private Transform target, objectTransform;
        private Quaternion currentRotation;
	
        private void Awake () {
            objectTransform = transform;
            target = objectTransform.parent;
            currentRotation = objectTransform.rotation;
        }

        private void LateUpdate () => SmoothUpdate();       

        private void SmoothUpdate()
        {
            currentRotation = Smooth();
            objectTransform.rotation = currentRotation;
        }

        private Quaternion Smooth() => Quaternion.Slerp(currentRotation, target.rotation, Time.deltaTime * smoothSpeed);
    }
}