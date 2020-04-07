using UnityEngine;

namespace AI.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/PatrolState")]
    public class PatrolState : AiBaseState
    {
        [SerializeField] private Vector3[] patrolPoints;
        [SerializeField] private float jumpDistance;
        [SerializeField] private float moveSpeed;

        private int currentPoint = 0;
        
        public override void Enter()
        {
            Ai.moveSpeed = moveSpeed;
            ChooseClosest();
        }

        public override void Run()
        {
            if (patrolPoints.Length > 0)
            {
                Ai.agent.SetDestination(patrolPoints[currentPoint]);
                if (Vector3.Distance(Ai.transform.position, patrolPoints[currentPoint]) < 1)
                    currentPoint = (currentPoint + 1) % patrolPoints.Length;
            }
            if (CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < jumpDistance)
                stateMachine.TransitionTo<JumpState>();
        }
        
        private void ChooseClosest()
        {
            int closest = 0;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                float dist = Vector3.Distance(Ai.transform.position, patrolPoints[i]);
                if (dist < Vector3.Distance(Ai.transform.position, patrolPoints[closest]))
                    closest = i;
            }
            currentPoint = closest;
        }
    }
}

