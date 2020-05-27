using UnityEngine;

namespace Interactables.Triggers.Platform
{
    public class DisablePlattformsListener : MonoBehaviour
    {
        [SerializeField] private Material material;
        private MeshRenderer _renderer;
        private BoxCollider _collider;
        private void Start()
        {
            DisablePlattforms.onDisablePlattformsEvent += DisablePlattform;
            _renderer = transform.parent.GetChild(0).GetComponent<MeshRenderer>();
            _collider = GetComponent<BoxCollider>();
        }

        private void OnDestroy()
        {
            DisablePlattforms.onDisablePlattformsEvent -= DisablePlattform;
        }
    
        private void DisablePlattform()
        {
            _renderer.material = material;
            _collider.enabled = false;
        }
    }
}
