﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorControllerStateMachine
{
    [CreateAssetMenu(menuName = "MovingDoorStates/ClosingState")]
    public class ClosingDoorState : DoorControllerBaseState
    {

        // Start is called before the first frame update
        public override void Enter()
        {
            Debug.Log("Door Enterd Closing State");
        }

        // Update is called once per frame
        public override void Run()
        {
            //MovingDoor.transform.position = Vector3.down; //Ta bort och animera istället
            //stateMachine.TransitionTo<ClosedDoorState>();
        }
    }
}
