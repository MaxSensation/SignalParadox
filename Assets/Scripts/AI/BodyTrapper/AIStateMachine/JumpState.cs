using AI.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/JumpState")]
    public class JumpState : BodyTrapperBaseState
    {
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpLength;


        public override void Enter()
        {
            base.Enter();
            Jump();
            Ai.ActivateStun();
        }

        public override void Exit()
        {
            base.Exit();
            Ai.agent.ResetPath();
        }

        public override void Run()
        {
            if (Ai.isDead)
                stateMachine.TransitionTo<DeadState>();
            
            Ai.TouchingPlayer();
            
            if (Grounded() && !Ai.IsStunned())
            {
                Ai.agent.isStopped = false;
                stateMachine.TransitionTo<HuntState>();
            }
        }

        private bool Grounded()
        {
            return Physics.Raycast(Ai.transform.position, Vector3.down, 0.3f, Ai.agent.areaMask);
        }

        private void Jump()
        {
            Ai.rigidbody.velocity = Ai.jumpDirection * jumpLength + Vector3.up * (jumpHeight);
        }
    }
}
