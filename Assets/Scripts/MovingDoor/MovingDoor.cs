using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDoor : MonoBehaviour
{
    public State[] states;
    private StateMachine _stateMachine;
    private BoxCollider _collider;
    private GameObject _triggerCollider;
    private RaycastHit _PlayertriggeredDoor;
    private bool _hasButton;

    public void Awake()
    {
        _hasButton = false; //..
        _stateMachine = new StateMachine(this, states);
        _collider = GetComponent<BoxCollider>();
        _triggerCollider = transform.Find("DoorTrigger").gameObject;
    }

    public void Start()
    {
        if (!_hasButton)
        {
            _triggerCollider.SetActive(true);
        }
    }

    void Update()
    {
        _stateMachine.Run();
        _PlayertriggeredDoor = _triggerCollider.GetComponent<TriggerCollider>().PlayerTriggered();
    }

    internal RaycastHit GetTriggerCollider()
    {
            return _PlayertriggeredDoor;
    }
}
