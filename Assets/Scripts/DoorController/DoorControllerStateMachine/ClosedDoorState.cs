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
            if (Door.GetHasButton() && Door.CloseDoor() && !Door.OpenDoor())
            {
                Debug.Log("Player Pressed Button");
                Door.gameObject.layer = 1; //Temporärt annars kommer dörren annars kommer dörre krocka med taket.
                stateMachine.TransitionTo<OpeningDoorState>();
            }
            else if (!Door.GetHasButton() && Door.GetPlayerTriggeredCast() && Door.CloseDoor() && !Door.OpenDoor())
            {
                Debug.Log("Player Triggered Door Cast");
                Door.gameObject.layer = 1; //Temporärt annars kommer dörren annars kommer dörre krocka med taket.
                stateMachine.TransitionTo<OpeningDoorState>();
            }
        }
    }
}

