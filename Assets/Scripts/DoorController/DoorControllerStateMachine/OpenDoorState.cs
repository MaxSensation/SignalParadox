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
            if (Door.GetHasButton() && !Door.CloseDoor() && Door.OpenDoor())
            {
                stateMachine.TransitionTo<ClosingDoorState>();
            }
            else if (!Door.GetHasButton() && Door.GetPlayerTriggeredCast() && !Door.CloseDoor() && Door.OpenDoor())
            {
                stateMachine.TransitionTo<ClosingDoorState>();
            }
        }
    }
}