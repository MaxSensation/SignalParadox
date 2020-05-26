//Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/StunState")]
    public class StunState : ChargerBaseState
    {


        public override void Enter()
        {
            base.Enter();
            Ai.ActivateStun();
        }

        public override void Run()
        {
            if (!Ai.IsStunned)
                stateMachine.TransitionTo<HuntState>();
        }
    }
}