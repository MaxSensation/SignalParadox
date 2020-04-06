using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public State[] states;
    private StateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = new StateMachine(this, states);
    }

    private void Update()
    {
        _stateMachine.Run();
        //GetplayerpressedButton
    }

}
