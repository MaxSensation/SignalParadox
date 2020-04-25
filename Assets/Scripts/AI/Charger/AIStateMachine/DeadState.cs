using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/DeadState")]
    public class DeadState : ChargerBaseState
    {
        public override void Enter()
        {
            Debug.Log("Charger is dead");
            Ai.agent.enabled = false;
            base.Enter();
        }
    }
}