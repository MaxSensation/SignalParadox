using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollider : MonoBehaviour
{
    [SerializeField] private bool _triggered;
    [SerializeField] private LayerMask _layerMask;
    public BoxCollider _collider = null;
    private RaycastHit _boxCastHit;
    //private bool _hit;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        //_triggered = PlayerTriggered().collider;
    }

    internal RaycastHit PlayerTriggered()
    {
        Physics.BoxCast(_collider.bounds.center, transform.localScale, transform.up, out _boxCastHit, transform.rotation, 5f, _layerMask, QueryTriggerInteraction.Collide);
        return _boxCastHit;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (_boxCastHit.collider)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.up * _boxCastHit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.up * _boxCastHit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.up * 5f);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.up * 5f, transform.localScale);
        }
    }
}
