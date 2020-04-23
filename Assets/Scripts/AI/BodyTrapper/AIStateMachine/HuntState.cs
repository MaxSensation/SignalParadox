using AI.AIStateMachine;
using AI.Charger.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/HuntState")]
    public class HuntState : BodyTrapperBaseState
    {
        [SerializeField] private float jumpDistance;
        [SerializeField] private float searchingRange;
        public override void Run()
        {
            if (Ai.isDead)
                stateMachine.TransitionTo<DeadState>();
            
            if (!Ai.IsStunned())
                Ai.agent.SetDestination(Ai.target.transform.position);                
            
            if (CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < jumpDistance)
                stateMachine.TransitionTo<JumpState>();
            
            if (!CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) > searchingRange)
                stateMachine.TransitionTo<PatrolState>();
            
        }
    }
}