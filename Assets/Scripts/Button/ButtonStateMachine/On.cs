using System.Collections;
using System.Collections.Generic;
using EventSystem;
using UnityEngine;

namespace ButtonStateMachine
{
    [CreateAssetMenu(menuName = "ButtonStates/OnState")]
    public class On : ButtonBaseState
    {
        public override void Enter()
        {
            Debug.Log("Button on");
            base.Enter();
        }

        public override void Run()
        {
            if (!Button.IsOffCooldown() && Button.IsOnCooldown())
            {
                //Button.Door.ActivateDoor();
                EventHandler.InvokeEvent(new OnButtonPressedEvent(_button.interactableObject));
                stateMachine.TransitionTo<Off>();
            }        
        }

    }
}

