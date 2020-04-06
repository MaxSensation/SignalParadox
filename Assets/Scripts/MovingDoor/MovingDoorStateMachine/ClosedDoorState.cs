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
            if (MovingDoor.GetTriggerCollider().collider)
                stateMachine.TransitionTo<OpeningDoorState>();
            //else
            //    Debug.Log("not triggered");
        }
    }
}

