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
            Button.Door.ActivateDoor();
            //Button.SetRenderColor("_Color", Color.green);
        }

        public override void Run()
        {
            if (!Button.IsOffCooldown() && Button.IsOnCooldown())
                stateMachine.TransitionTo<Off>();            
        }

    }
}

