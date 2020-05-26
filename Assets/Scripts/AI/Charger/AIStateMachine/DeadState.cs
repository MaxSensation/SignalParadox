//Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/DeadState")]
    public class DeadState : ChargerBaseState
    {
        public override void Enter()
        {
            Ai.agent.enabled = false;
            base.Enter();
        }
    }
}