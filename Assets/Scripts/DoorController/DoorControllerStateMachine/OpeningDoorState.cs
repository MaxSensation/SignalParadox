using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorControllerStateMachine
{
    [CreateAssetMenu(menuName = "MovingDoorStates/OpeningState")]
    public class OpeningDoorState : DoorControllerBaseState

    {
        [SerializeField] private IEnumerator _coroutine;

        public override void Enter()
        {
            Debug.Log("Door Enterd Opening State");
        }

        public override void Run()
        {
            DoorController.transform.position = Vector3.up;
            stateMachine.TransitionTo<OpenDoorState>();
        }
    }
}