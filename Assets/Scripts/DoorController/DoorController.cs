using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public State[] states;
    private StateMachine _stateMachine;
    private BoxCollider _collider;
    private Vector3 _triggerPosition;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _hasButton;
    private bool _closeDoor;
    private bool _openDoor;

    private void Awake()
    {
        //_pushedButton = false;
        _hasButton = true;
        _stateMachine = new StateMachine(this, states);
        _collider = GetComponent<BoxCollider>();
        _triggerPosition = (_collider.transform.position + (_collider.size.y * 2) * Vector3.down);
    }

    private void Update()
    {
        _stateMachine.Run();
    }

    internal bool GetPlayerTriggeredCast()
    {
        Physics.BoxCast(_triggerPosition, new Vector3(1, 1, 1) * 1f, Vector3.up, out var _boxCastHit, transform.rotation, 5f, _layerMask, QueryTriggerInteraction.Collide);
        return _boxCastHit.collider && _boxCastHit.collider.CompareTag("Player") && !_hasButton;
    }

    internal IEnumerator CooldownTime()
    {
        yield return new WaitForSeconds(2);
        _closeDoor = false;
        _openDoor = false;
        StopCoroutine("ActivateDoor");
    }

    internal void ActivateDoor()
    {
        if (_stateMachine.GetCurrentState().name.Equals("ClosedState(Clone)"))
        {
            _closeDoor = true;
        }
        else
        {
            _openDoor = true;
        }
        StartCoroutine("CooldownTime");
    }

    internal bool CloseDoor()
    {
        return _closeDoor;
    }

    internal bool OpenDoor()
    {
        return _openDoor;
    }

    internal bool GetHasButton()
    {
        return _hasButton;
    }

    //internal bool SetPushedButton(bool value)
    //{
    //   return _pushedButton = value;
    //}
}
