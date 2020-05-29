//Main author: Maximiliam Rosén

using UnityEngine;

namespace TransformModifier
{
    public class RotationRandomizer : MonoBehaviour
    {
        [SerializeField] [Range(0f,100f)] private float rotationSpeed;
        private void Update() => transform.Rotate(0,rotationSpeed * Time.deltaTime,0);
    }
}