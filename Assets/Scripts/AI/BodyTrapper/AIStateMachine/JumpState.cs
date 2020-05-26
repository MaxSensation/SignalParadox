//Main author: Maximiliam Rosén

using System;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/JumpState")]
    public class JumpState : BodyTrapperBaseState
    {
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpLength;
        [SerializeField] private AudioClip jumpSound;

        public static Action<GameObject> onJumpEvent;

        private void AddJumpForce() => Ai.aiRigidbody.velocity = Ai.jumpDirection * jumpLength + Vector3.up * (jumpHeight);

        public override void Enter()
        {
            base.Enter();
            Ai.audioSource.PlayOneShot(jumpSound);
            AddJumpForce();
            onJumpEvent?.Invoke(Ai.gameObject);
            Ai.canAttack = true;
        }

        public override void Run()
        {
            Ai.TouchingPlayer();
            if (Ai.Grounded() && Vector3.Dot(Vector3.up, Ai.aiRigidbody.velocity) <= 0 || !Ai.isStuckOnPlayer && !Ai.canAttack)
                stateMachine.TransitionTo<StunState>();
        }
    }
}
