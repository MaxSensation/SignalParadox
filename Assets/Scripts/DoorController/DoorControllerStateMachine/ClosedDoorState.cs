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
            if (!Door.GetHasButtonAndIsPushed() && Door.GetPlayerTriggeredCast())
            {
                /* MovingDoor.transform.position = Vector3.up; */// Ta bort och animera istället
                Debug.Log("Player Triggered Door Cast");
                stateMachine.TransitionTo<OpeningDoorState>();
            }
            else if (Door.GetHasButtonAndIsPushed())
            {
                Debug.Log("Player Pressed Button");
                Door.gameObject.layer = 1; //Temporärt annars kommer dörren annars kommer dörre krocka med taket.
                stateMachine.TransitionTo<OpeningDoorState>();
            }
        }
    }
}

