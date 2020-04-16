using System;
using System.Collections;
using System.Linq;
using EventSystem;
using UnityEngine;
using EventHandler = EventSystem.EventHandler;

public class DoorController : MonoBehaviour
{
    [SerializeField] private State[] states;
    private StateMachine _stateMachine;
    private Vector3 _triggerPosition;
    internal BoxCollider _collider;
    internal bool _isOpen;
    private bool _isMoving;

    private void Awake()
    {
        _stateMachine = new StateMachine(this, states);
    }

    private void Start()
    {
        EventHandler.RegisterListener<OnButtonPressedEvent>(ActivateButton);
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterListener<OnButtonPressedEvent>(ActivateButton);
    }

    private void ActivateButton(OnButtonPressedEvent obj)
    {
        if (obj.interactableObjects.Contains(gameObject) && !_isMoving)
        {
            StartCoroutine("CooldownTime");
        }
    }

    private void Update()
    {
        _stateMachine.Run();
    }

    internal IEnumerator CooldownTime()
    {
        _isMoving = true;
        _isOpen = !_isOpen;
        yield return new WaitForSeconds(2);
        _isMoving = false;
    }
}
