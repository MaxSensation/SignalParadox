﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/ChargeUpState")]
    public class ChargeUpState : AiBaseState
    {

        public override void Enter()
        {
            base.Enter();
            Ai.ChargeUp();
        }

        public override void Run()
        {
            if (Ai.hasChargedUp)
                stateMachine.TransitionTo<ChargeState>();
        }

    }
}
