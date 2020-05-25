//Main author: Andreas Berzelius

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
    }
}
