using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorControllerStateMachine
{
    [CreateAssetMenu(menuName = "MovingDoorStates/OpenState")]
    public class OpenDoorState : DoorControllerBaseState
    {

        public override void Enter()
        {
            Debug.Log("Door Enterd Open State");
        }

        public override void Run()
        {
            //stateMachine.TransitionTo<ClosingDoorState>(); //Ta bort bara 
        }
    }
}