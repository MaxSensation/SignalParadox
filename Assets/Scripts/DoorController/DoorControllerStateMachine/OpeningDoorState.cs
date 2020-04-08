using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorControllerStateMachine
{
    [CreateAssetMenu(menuName = "MovingDoorStates/OpeningState")]
    public class OpeningDoorState : DoorControllerBaseState

    {
        public override void Enter()
        {
            Debug.Log("Door Enterd Opening State");
        }

        public override void Run()
        {
            Door.transform.position = new Vector3(Door.transform.position.x,Door.transform.position.y + 2, Door.transform.position.z);
            stateMachine.TransitionTo<OpenDoorState>();
        }
    }
}