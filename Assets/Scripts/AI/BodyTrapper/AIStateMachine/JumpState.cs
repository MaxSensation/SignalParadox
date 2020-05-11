//Main author: Maximiliam Rosén

using AI.AIStateMachine;
using System;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/JumpState")]
    public class JumpState : BodyTrapperBaseState
    {
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpLength;
        public static Action<GameObject> onJumpEvent;
        public static Action<GameObject> onLandEvent;

        public override void Enter()
        {
            base.Enter();
            Jump();
            onJumpEvent?.Invoke(Ai.gameObject);
            Ai.canAttack = true;
        }
        
        public override void Run()
        {
            if (Ai.isDead)
                stateMachine.TransitionTo<DeadState>();
            
            Ai.TouchingPlayer();
            
            if (Grounded() && Vector3.Dot(Vector3.up, Ai.rigidbody.velocity) <= 0 || !Ai.isStuckOnPlayer && !Ai.canAttack)
            {
                stateMachine.TransitionTo<StunState>();
                onLandEvent?.Invoke(Ai.gameObject);
            }
        }

        private bool Grounded()
        {
            return Physics.Raycast(Ai.transform.position, Vector3.down, 0.1f, Ai.agent.areaMask);
        }

        private void Jump()
        {
            Ai.rigidbody.velocity = Ai.jumpDirection * jumpLength + Vector3.up * (jumpHeight);
        }
    }
}
