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
            Debug.Log("Button off");
            base.Enter();
        }

        public override void Run()
        {
            if(Button.IsOffCooldown() && !Button.IsOnCooldown())
            {
                //Button.Door.ActivateDoor(); //Om off knappen ska avaktivera saker.
                EventHandler.InvokeEvent(new OnButtonPressedEvent(_button.interactableObject));
                stateMachine.TransitionTo<On>();
            }
        }
    }
}

