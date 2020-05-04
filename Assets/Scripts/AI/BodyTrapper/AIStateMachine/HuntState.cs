//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using AI.AIStateMachine;
using UnityEngine;
using UnityEngine.AI;

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
            
            if (!Ai.IsStunned() && Ai.agent.enabled)
                Ai.agent.SetDestination(Ai.target.transform.position);
            
            if (Ai.LookingAtPlayer(Ai, maxMinLookRange) && CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < jumpDistance)
                stateMachine.TransitionTo<ChargeState>();
            NavMesh.CalculatePath(Ai.target.transform.position, Ai.transform.position, NavMesh.AllAreas, Ai.path);
            if (Ai.path.status != NavMeshPathStatus.PathComplete || (!CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) > searchingRange))
                stateMachine.TransitionTo<PatrolState>();
            
        }
    }
}