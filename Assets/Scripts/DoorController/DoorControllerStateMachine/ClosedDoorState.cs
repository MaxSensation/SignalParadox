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
            Debug.Log("Enterd Closed State");
        }

        public override void Run()
        {
            if (DoorController.PlayerTriggeredCast().collider && DoorController.PlayerTriggeredCast().collider.CompareTag("Player") && !DoorController.GetHasButton()) //Gör om så det inte blir två calls hela tiden kanske?
            {
                /* MovingDoor.transform.position = Vector3.up; */// Ta bort och animera istället
                Debug.Log("player Triggered Door Cast");
                stateMachine.TransitionTo<OpeningDoorState>();
            }
            else
                if (DoorController.GetHasButton())
            {
                Debug.Log("Player Pressed Button");
                stateMachine.TransitionTo<OpeningDoorState>();
            }
        }
    }
}

