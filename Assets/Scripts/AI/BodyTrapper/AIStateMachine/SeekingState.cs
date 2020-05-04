//Main author: Maximiliam Rosén

using AI.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/SeekingState")]
    public class SeekingState : BodyTrapperBaseState
    {
        public override void Run()
        {
            if (Ai.lastSoundLocation != Vector3.zero)
            {
                if (Ai.agent.enabled && Ai.agent.isStopped == false)
                {
                    Ai.agent.destination = Ai.lastSoundLocation;
                    if (CanSeePlayer() && (Ai.target.transform.position - Ai.transform.position).magnitude < 4f)
                    {
                        stateMachine.TransitionTo<HuntState>();
                    }
                }
            }
            else
            {
                stateMachine.TransitionTo<PatrolState>();
            }
        }
    }
}