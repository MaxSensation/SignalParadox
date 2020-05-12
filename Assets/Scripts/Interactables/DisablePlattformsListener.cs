using UnityEngine;

public class DisablePlattformsListener : MonoBehaviour
{
    [SerializeField] private Material material;
    private MeshRenderer _renderer;
    private BoxCollider _collider;
    private void Start()
    {
        DisablePlattforms.OnDisablePlattformsEvent += DisablePlattform;
        _renderer = transform.parent.GetChild(0).GetComponent<MeshRenderer>();
        _collider = GetComponent<BoxCollider>();
    }

    private void OnDestroy()
    {
        DisablePlattforms.OnDisablePlattformsEvent -= DisablePlattform;
    }
    
    private void DisablePlattform()
    {
        _renderer.material = material;
        _collider.enabled = false;
    }
}
