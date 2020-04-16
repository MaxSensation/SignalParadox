using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
            Door.gameObject.layer = 1; //Det här kan bli fel om layers ordning ändras om
            stateMachine.TransitionTo<OpenDoorState>();
        }
    }
}