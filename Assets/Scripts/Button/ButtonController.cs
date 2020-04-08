using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public State[] states;
    private StateMachine _stateMachine;
    //internal bool pushed;
    public DoorController Door;
    private bool _offcooldown;
    private bool _onCooldown;
    [SerializeField] internal Material _OffMaterial;
    [SerializeField] internal Material _OnMaterial;
    internal MeshRenderer _currentMaterial;

    private void Awake()
    {
        _stateMachine = new StateMachine(this, states);
        _currentMaterial = GetComponent<MeshRenderer>();
        //_currentMaterial.material.SetColor("_Color", Color.red);
    }

    internal void SetRenderColor(string name, Color value)
    {
        _currentMaterial.material.SetColor(name, value);
    }

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




