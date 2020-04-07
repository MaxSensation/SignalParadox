using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public State[] states;
    private StateMachine _stateMachine;
    private BoxCollider _collider;
    [SerializeField] private LayerMask _layerMask;
    //private RaycastHit _boxCastHit;
    private Vector3 _triggerPosition;
    private bool _hasButton;
    private bool _pushedButton;

    private void Awake()
    {
        //_pushedButton = false;
        _hasButton = true; //..
        _stateMachine = new StateMachine(this, states);
        _collider = GetComponent<BoxCollider>();
        _triggerPosition = (_collider.transform.position + (_collider.size.y * 2) * Vector3.down);
    }

    private void Update()
    {
        _stateMachine.Run();
    }

    //internal RaycastHit PlayerTriggeredCast() //Fixa denna den kommer ändå returnera fastän boxcasten e tom
    //{
    //    if (!_hasButton)
    //    {
    //        Physics.BoxCast(_triggerPosition, new Vector3(1, 1, 1) * 1f, Vector3.up, out RaycastHit _boxCastHit, transform.rotation, 5f, _layerMask, QueryTriggerInteraction.Collide);
    //        return _boxCastHit;
    //    }
    //    return _boxCastHit;
    //}

    internal bool GetPlayerTriggeredCast()
    {
        Physics.BoxCast(_triggerPosition, new Vector3(1, 1, 1) * 1f, Vector3.up, out var _boxCastHit, transform.rotation, 5f, _layerMask, QueryTriggerInteraction.Collide);
        return _boxCastHit.collider && _boxCastHit.collider.CompareTag("Player") && !_hasButton;
    }

    internal BoxCollider GetCollider()
    {
        return _collider;
    }

    internal void SetCollider(bool value)
    {
        _collider.isTrigger = value;
    }

    internal bool GetHasButtonAndIsPushed()
    {
        if (_hasButton)
            return _pushedButton;
        else
            return false;
    }

    internal bool SetPushedButton(bool value)
    {
       return _pushedButton = value;
    }
}
