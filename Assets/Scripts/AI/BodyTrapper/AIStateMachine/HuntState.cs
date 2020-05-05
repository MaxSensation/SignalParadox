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
            if (CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < jumpDistance && Ai.LookingAtPlayer(Ai, maxMinLookRange))
                stateMachine.TransitionTo<ChargeState>();

            if (CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < jumpDistance)
            {
                var e = (Ai.target.transform.position - Ai.transform.position);
                var targetRotation = Quaternion.LookRotation(new Vector3(e.x, 0, e.z), Vector3.up);
                Ai.transform.rotation = Quaternion.Lerp(Ai.transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            NavMesh.CalculatePath(Ai.target.transform.position, Ai.transform.position, NavMesh.AllAreas, Ai.path);
            if (Ai.path.status != NavMeshPathStatus.PathComplete || (!CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) > searchingRange))
                stateMachine.TransitionTo<PatrolState>();
            
        }
    }
}