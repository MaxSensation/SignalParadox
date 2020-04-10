using AI.AIStateMachine;
using AI.Charger.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/JumpState")]
    public class JumpState : AiBaseState
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
            
            if (TouchingPlayer())
            {
                Ai.target.GetComponent<PlayerController>().Die();
            }
            if (Grounded() && !Ai.IsStunned())
            {
                stateMachine.TransitionTo<HuntState>();
            }
        }

        private bool Grounded()
        {
            return Physics.Raycast(Ai.transform.position, Vector3.down, 0.3f, Ai.agent.areaMask);
        }

        private bool TouchingPlayer()
        {
            Physics.Raycast(Ai.transform.position, (Ai.target.transform.position - Ai.transform.position).normalized, out var hit, 1f);
            return (hit.collider && hit.collider.CompareTag("Player"));
        }

        private void Jump()
        {
            Ai.rigidbody.velocity = (Ai.target.transform.position - Ai.transform.position).normalized * jumpHeight;
        }
    }
}
