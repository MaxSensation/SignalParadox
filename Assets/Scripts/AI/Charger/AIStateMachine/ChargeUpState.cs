using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/ChargeUpState")]
    public class ChargeUpState : ChargerBaseState
    {

        public override void Enter()
        {
            base.Enter();
            Ai.ChargeUp();
            Ai.agent.enabled = false;
        }

        public override void Run()
        {
            if (Ai.GetHasChargedUp())
            {
                stateMachine.TransitionTo<ChargeState>();
            }
        }

    }
}
