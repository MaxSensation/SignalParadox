using System.Collections;
using System.Collections.Generic;
using EventSystem;
using UnityEngine;

namespace ButtonStateMachine
{
    [CreateAssetMenu(menuName = "ButtonStates/OffState")]
    public class Off : ButtonBaseState
    {
        public override void Enter()
        {
            
        }

        public override void Run()
        {
            if(Button.IsOffCooldown() && !Button.IsOnCooldown())
            {
                EventHandler.InvokeEvent(new OnButtonPressedEvent(_button.interactableObject));
                stateMachine.TransitionTo<On>();
            }
        }
    }
}

