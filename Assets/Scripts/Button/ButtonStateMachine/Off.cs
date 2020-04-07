using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ButtonStateMachine
{
    [CreateAssetMenu(menuName = "ButtonStates/OffState")]
    public class Off : ButtonBaseState
    {
        public override void Enter()
        {
            Debug.Log("Button off");
        }

        public override void Run()
        {
            //if ispressed transition to On..
            if(Button.pushed) //Ändra
                stateMachine.TransitionTo<On>();
        }
    }
}

