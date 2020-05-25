//Main author: Maximiliam Rosén
using System;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/StunState")]
    public class StunState : BodyTrapperBaseState
    {
        [SerializeField] private AudioClip stunnedSound;
        
        public static Action<GameObject> onLandEvent;

        public override void Enter()
        {
            base.Enter();
            Ai.aiRigidbody.velocity = Vector3.zero;
            Ai.agent.enabled = false;
            onLandEvent?.Invoke(Ai.gameObject);
            Ai.audioSource.PlayOneShot(stunnedSound,1f);
            Ai.ActivateStun();
        }

        public override void Run()
        {
            if (!Ai.IsStunned)
                stateMachine.TransitionTo<PatrolState>();
        }

        public override void Exit()
        {
            base.Exit();
            Ai.aiRigidbody.useGravity = true;
            Ai.agent.enabled = true;
        }
    }
}