using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public State[] states;
    private StateMachine _stateMachine;
    public DoorController Door;
    private bool _offcooldown;
    private bool _onCooldown;
    private Renderer _buttonRenderer;

    private void Awake()
    {
        _buttonRenderer = GetComponent<Renderer>();
        _stateMachine = new StateMachine(this, states);

    }

    internal Renderer GetRenderer()
    {
        return _buttonRenderer;
    }

    //internal void SetRenderColor(string name, Color value)
    //{
    //    _currentMaterial.material.SetColor(name, value);
    //}

    private void Update()
    {
        _stateMachine.Run();
    }

    internal bool IsOffCooldown()
    {
        return _offcooldown;
    }

    internal bool IsOnCooldown()
    {
        return _onCooldown;
    }

    internal State GetCurrentButtonState()
    {
        return _stateMachine.GetCurrentState();
    }

    internal IEnumerator ActivateButton()
    {
        yield return new WaitForSeconds(2);
        _offcooldown = false;
        _onCooldown = false;
        StopCoroutine("ActivateButton");
    }

    internal void ButtonPress()
    {
        if (_stateMachine.GetCurrentState().name.Equals("OffState(Clone)"))
        {
            _offcooldown = true;
        }
        else
        {
            _onCooldown = true;
        }
        StartCoroutine("ActivateButton");
    }

}




