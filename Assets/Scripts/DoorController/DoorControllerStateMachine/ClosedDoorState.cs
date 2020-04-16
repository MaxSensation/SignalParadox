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
            if (Door._isOpen)
            {
                Debug.Log("Player Pressed Button");
                stateMachine.TransitionTo<OpeningDoorState>();
            }
        }
    }
}

