using UnityEngine;

namespace Interactables.Triggers.Platform
{
    public class DisablePlattformsListener : MonoBehaviour
    {
        [SerializeField] private Material material;
        private MeshRenderer plattformRenderer;
        private BoxCollider plattformCollider;
        private void Start()
        {
            DisablePlattforms.onDisablePlattformsEvent += DisablePlattform;
            plattformRenderer = transform.parent.GetChild(0).GetComponent<MeshRenderer>();
            plattformCollider = GetComponent<BoxCollider>();
        }

        private void OnDestroy() => DisablePlattforms.onDisablePlattformsEvent -= DisablePlattform;
    
        private void DisablePlattform()
        {
            plattformRenderer.material = material;
            plattformCollider.enabled = false;
        }
    }
}
