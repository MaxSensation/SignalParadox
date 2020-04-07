using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public State[] states;
    private StateMachine _stateMachine;
    internal bool pushed;
    public DoorController Door;

    private void Awake()
    {
        _stateMachine = new StateMachine(this, states);
    }

    private void Update()
    {
        _stateMachine.Run();
        //GetplayerpressedButton
    }

    internal void Pushed()
    {
        //if(_stateMachine.GetCurrentState().GetType().Equals()) ....
        pushed = true;
    }   
}
