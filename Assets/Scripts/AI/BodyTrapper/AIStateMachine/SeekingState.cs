//Main author: Maximiliam Rosén
using System;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/SeekingState")]
    public class SeekingState : BodyTrapperBaseState
    {
        public static Action<GameObject> onIdleEvent, onMovingEvent;
        
        public override void Run()
        {
            if (Ai.agent.velocity == Vector3.zero)
                onIdleEvent?.Invoke(Ai.gameObject);
            else
                onMovingEvent?.Invoke(Ai.gameObject);

            if (Ai.lastSoundLocation != Vector3.zero)
            {
                if (!Ai.agent.enabled || Ai.agent.isStopped) return;
                Ai.agent.destination = Ai.lastSoundLocation;
                if (Ai.echoLocationResult.Transmitter == null) return;
                if (!Ai.hasHeardDecoy && Ai.echoLocationResult.Transmitter.CompareTag("Player") && Ai.echoLocationResult.BestSoundStrength > 0.95f)
                    stateMachine.TransitionTo<HuntState>();
            }
            else
            {
                onMovingEvent?.Invoke(Ai.gameObject);
                stateMachine.TransitionTo<PatrolState>();
            }
        }
    }
}