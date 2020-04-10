using UnityEngine;

public class TriggerCheckPoint : MonoBehaviour
{
    private Collider _collider;
    private bool _checkPointUsed;
    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_checkPointUsed)
        {
            CheckForPlayer();            
        }
    }

    private void CheckForPlayer()
    {
        if (Physics.BoxCast(_collider.bounds.center, _collider.bounds.extents, Vector3.up, out var hit, _collider.transform.rotation, 5f))
        {
            if (hit.collider && hit.collider.CompareTag("Player"))
            {
                SaveManager.checkPointReached.Invoke();
                _checkPointUsed = true;
            }
        }
    }
}
