using AI.AIStateMachine;
using AI.Charger.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/JumpState")]
    public class JumpState : BodyTrapperBaseState
    {
        [SerializeField] private float jumpHeight;


        public override void Enter()
        {
            base.Enter();
            Jump();
            Ai.ActivateStun();
        }

        public override void Run()
        {
            if (Ai.isDead)
                stateMachine.TransitionTo<DeadState>();
            
            Ai.TouchingPlayer();
            
            if (Grounded() && !Ai.IsStunned())
            {
                stateMachine.TransitionTo<HuntState>();
            }
        }

        private bool Grounded()
        {
            return Physics.Raycast(Ai.transform.position, Vector3.down, 0.3f, Ai.agent.areaMask);
        }

        private void Jump()
        {
            Ai.rigidbody.velocity = (Ai.target.transform.position - Ai.transform.position).normalized * jumpHeight;
        }
    }
}
