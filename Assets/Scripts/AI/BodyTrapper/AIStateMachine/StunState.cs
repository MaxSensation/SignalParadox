//Main author: Maximiliam Rosén
using System;
using AI.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/StunState")]
    public class StunState : BodyTrapperBaseState
    {
        public static Action<GameObject> onLandEvent;
        [SerializeField] private AudioClip stunnedSound;

        public override void Enter()
        {
            base.Enter();
            Ai.rigidbody.velocity = Vector3.zero;
            Ai.agent.enabled = false;
            Ai.rigidbody.useGravity = false;
            onLandEvent?.Invoke(Ai.gameObject);
            Ai.audioSource.PlayOneShot(stunnedSound,1f);
            Ai.ActivateStun();
        }

        public override void Run()
        {
            Ai.rigidbody.velocity = Vector3.zero;
            if (!Ai._stunned)
            {
                stateMachine.TransitionTo<PatrolState>();
            }
        }

        public override void Exit()
        {
            base.Exit();
            Ai.rigidbody.useGravity = true;
            Ai.agent.enabled = true;
        }
    }
}