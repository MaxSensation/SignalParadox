using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovingDoorStateMachine
{
    [CreateAssetMenu(menuName = "MovingDoorStates/OpenState")]
    public class OpenDoorState : MovingDoorBaseState
    {

        public override void Enter()
        {
            Debug.Log("Enterd Open State");
        }

        public override void Run()
        {

        }
    }
}