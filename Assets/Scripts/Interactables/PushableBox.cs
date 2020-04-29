using System;
using UnityEngine;

namespace Interactables
{
    internal class BoxWithSides
    {
        internal Vector3 left, right, forward, back;
        internal BoxWithSides(Vector3 left, Vector3 right, Vector3 forward, Vector3 back)
        {
            this.left = left;
            this.right = right;
            this.forward = forward;
            this.back = back;
        }
    } 

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class PushableBox : MonoBehaviour, IPushable
    {
        private BoxCollider _boxCollider;
        private Rigidbody _rigidbody;
        private Vector3 _checkSize;
        private BoxWithSides _boxWithSides;

        private void Awake()
        {
            _checkSize = new Vector3(1f,1.8f, 1f);
            _boxCollider = GetComponent<BoxCollider>();
            _rigidbody = GetComponent<Rigidbody>();
            const float sideSize = 1.4f;
            _boxWithSides = new BoxWithSides(
                _boxCollider.bounds.center - transform.right * sideSize,
                _boxCollider.bounds.center + transform.right * sideSize,
                _boxCollider.bounds.center + transform.forward * sideSize,
                _boxCollider.bounds.center - transform.forward * sideSize
            );
        }

        public void GetPushLocation()
        {
                
        }

        public void Push()
        {
        
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_boxWithSides.forward, _checkSize);
            Gizmos.DrawWireCube(_boxWithSides.right, _checkSize);
            Gizmos.DrawWireCube(_boxWithSides.left, _checkSize);
            Gizmos.DrawWireCube(_boxWithSides.back, _checkSize);
        
        }
    }
}