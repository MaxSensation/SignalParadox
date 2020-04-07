using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorControllerStateMachine
{
    [CreateAssetMenu(menuName = "MovingDoorStates/ClosedState")]
    public class ClosedDoorState : DoorControllerBaseState
    {
        private RaycastHit _hit;
        public override void Enter()
        {
            Debug.Log("Door Enterd Closed State");
        }

        public override void Run()
        {
            if (!DoorController.GetHasButtonAndIsPushed() && DoorController.GetPlayerTriggeredCast())
            {
                /* MovingDoor.transform.position = Vector3.up; */// Ta bort och animera istället
                Debug.Log("player Triggered Door Cast");
                stateMachine.TransitionTo<OpeningDoorState>();
            }
            else if (DoorController.GetHasButtonAndIsPushed())
            {
                Debug.Log("Player Pressed Button");
                stateMachine.TransitionTo<OpeningDoorState>();
            }
        }
    }
}

