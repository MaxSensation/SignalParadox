using UnityEngine;

namespace AI.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/HuntState")]
    public class HuntState : AiBaseState
    {
        [SerializeField] private float jumpDistance;

        public override void Run()
        {
            Ai.agent.SetDestination(Ai.target.transform.position);
            
            if (CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < jumpDistance)
                stateMachine.TransitionTo<JumpState>();
            
            if (!CanSeePlayer())
                stateMachine.TransitionTo<PatrolState>();
            
        }
    }
}