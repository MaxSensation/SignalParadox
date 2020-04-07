using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public State[] states;
    private StateMachine _stateMachine;
    private BoxCollider _collider;
    [SerializeField] private LayerMask _layerMask;
    private RaycastHit _boxCastHit;
    private Vector3 _triggerPosition;
    private bool _hasButton;

    private void Awake()
    {
        _hasButton = false; //..
        _stateMachine = new StateMachine(this, states);
        _collider = GetComponent<BoxCollider>();
        _triggerPosition = (_collider.transform.position + (_collider.size.y * 2) * Vector3.down);
    }

    private void Update()
    {
        _stateMachine.Run();
    }

    internal RaycastHit PlayerTriggeredCast() //Fixa denna den kommer ändå returnera fastän boxcasten e tom
    {
        if(!_hasButton)
        Physics.BoxCast(_triggerPosition, new Vector3(1,1,1) * 1f, Vector3.up , out _boxCastHit, transform.rotation, 5f, _layerMask, QueryTriggerInteraction.Collide);
        return _boxCastHit;
    }

    internal bool GetHasButton()
    {
        return _hasButton;
    }

    internal bool SetHasButton(bool value)
    {
       return _hasButton = value;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (_boxCastHit.collider)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(_triggerPosition, Vector3.up * 5f);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(_triggerPosition + Vector3.up * 5f, new Vector3(1, 1, 1) * 2f);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(_triggerPosition, Vector3.up * 5f);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(_triggerPosition + Vector3.up * 5f, new Vector3(1, 1, 1) * 2f);
        }
    }
}
