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
        [SerializeField] private float maxMinLookRange;
        public override void Run()
        {
            if (Ai.isDead)
                stateMachine.TransitionTo<DeadState>();
            
            if (!Ai.IsStunned())
                Ai.agent.SetDestination(Ai.target.transform.position);                
            
            if (Ai.LookingAtPlayer(Ai, maxMinLookRange) && CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < jumpDistance)
                stateMachine.TransitionTo<ChargeState>();
            
            if (!CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) > searchingRange)
                stateMachine.TransitionTo<PatrolState>();
            
        }
    }
}