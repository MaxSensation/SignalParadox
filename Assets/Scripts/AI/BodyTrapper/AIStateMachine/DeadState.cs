using AI.AIStateMachine;
using AI.Charger.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/DeadState")]
    public class DeadState : AiBaseState
    {
        public override void Enter()
        {
            Ai.agent.enabled = false;
            base.Enter();
        }
    }
}