//Main author: Maximiliam Rosén

using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/SeekingState")]
    public class SeekingState : BodyTrapperBaseState
    {
        [SerializeField] private float searchRange;

        public override void Run()
        {
            if (Ai.lastSoundLocation != Vector3.zero)
            {
                if (Ai.agent.enabled && Ai.agent.isStopped == false)
                {
                    Ai.agent.destination = Ai.lastSoundLocation;
                    if (Ai._echoLocationResult.Transmitter != null)
                        if (!Ai._hasHeardDecoy && Ai._echoLocationResult.Transmitter.CompareTag("Player") && Ai._echoLocationResult.BestSoundStrength > 0.95f)
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