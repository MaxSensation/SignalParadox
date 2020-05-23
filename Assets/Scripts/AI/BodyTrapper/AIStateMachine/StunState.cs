//Main author: Maximiliam Rosén
using System;
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
            Ai.aiRigidbody.velocity = Vector3.zero;
            Ai.agent.enabled = false;
            //Ai.aiRigidbody.useGravity = false;
            onLandEvent?.Invoke(Ai.gameObject);
            Ai.audioSource.PlayOneShot(stunnedSound,1f);
            Ai.ActivateStun();
        }

        public override void Run()
        {
            //Ai.aiRigidbody.velocity = Vector3.zero;
            if (!Ai.IsStunned())
            {
                stateMachine.TransitionTo<PatrolState>();
            }
        }

        public override void Exit()
        {
            base.Exit();
            Ai.aiRigidbody.useGravity = true;
            Ai.agent.enabled = true;
        }
    }
}