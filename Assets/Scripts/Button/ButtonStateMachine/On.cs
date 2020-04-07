using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ButtonStateMachine
{
    [CreateAssetMenu(menuName = "ButtonStates/OnState")]
    public class On : ButtonBaseState
    {
        public override void Enter()
        {
            Debug.Log("Button on");
        }

        public override void Run()
        {
            Button.Door.SetHasButton(true);
            //if (Button.pushed)
            //    stateMachine.TransitionTo<Off>();
        }

    }
}

