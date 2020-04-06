using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovingDoorStateMachine
{
    [CreateAssetMenu(menuName = "MovingDoorStates/OpeningState")]
    public class OpeningDoorState : MovingDoorBaseState

    {
        [SerializeField] private IEnumerator _coroutine;

        public override void Enter()
        {
            Debug.Log("Enterd Opening State");
        }

        public override void Run()
        {
            stateMachine.TransitionTo<OpenDoorState>();
        }
    }
}