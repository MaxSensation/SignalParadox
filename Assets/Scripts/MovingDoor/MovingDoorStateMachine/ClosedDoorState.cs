using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovingDoorStateMachine
{
    [CreateAssetMenu(menuName = "MovingDoorStates/ClosedState")]
    public class ClosedDoorState : MovingDoorBaseState
    {
        private RaycastHit _hit;
        public override void Enter()
        {
            Debug.Log("Enterd Closed State");
        }

        public override void Run()
        {
            if (MovingDoor.PlayerTriggeredCast().collider && MovingDoor.PlayerTriggeredCast().collider.CompareTag("Player"))
            {
                /* MovingDoor.transform.position = Vector3.up; */// Ta bort och animera istället
                Debug.Log("playerTriggeredDoor");
                stateMachine.TransitionTo<OpeningDoorState>();
            }
        }
    }
}

