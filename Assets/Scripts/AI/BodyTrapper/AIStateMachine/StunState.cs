//Main author: Maximiliam Rosén

using AI.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/StunState")]
    public class StunState : BodyTrapperBaseState
    {
        public override void Enter()
        {
            base.Enter();
            Ai.ActivateStun();
        }

        public override void Run()
        {
            if (!Ai._stunned)
            {
                stateMachine.TransitionTo<PatrolState>();
            }
        }
    }
}