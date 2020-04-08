using System.Collections;
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
            Door.transform.position = new Vector3(Door.transform.position.x, Door.transform.position.y - 2, Door.transform.position.z);
            Door.gameObject.layer = 8; //Det här kan bli fel om layers ordning ändras om
            stateMachine.TransitionTo<ClosedDoorState>();
        }
    }
}
