using UnityEngine;
using UnityEngine.Events;

public class PlatformTrigger : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private Material on;
    [SerializeField] private Material off;
    private MeshRenderer _meshRenderer;
    private static readonly int IsPressed = Animator.StringToHash("IsPressed");
    public UnityEvent isOn;
    public UnityEvent isOff;

    private void Awake()
    {
        var parent = transform.parent;
        _animator = parent.GetComponent<Animator>();
        _meshRenderer = parent.transform.Find("PlatformButton").GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool(IsPressed, true);
            _meshRenderer.material = on;
            isOn.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool(IsPressed, false);
            _meshRenderer.material = off;
            isOff.Invoke();
        }
    }
}
