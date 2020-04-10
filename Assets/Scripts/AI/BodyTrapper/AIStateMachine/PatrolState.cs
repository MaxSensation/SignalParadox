using AI.AIStateMachine;
using AI.Charger.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/PatrolState")]
    public class PatrolState : AiBaseState
    {
        [SerializeField] private float hearDistance;
        [SerializeField] private float seeDistance;

        private int currentPoint;
        
        public override void Enter()
        {
            base.Enter();
            ChooseClosest();
        }

        public override void Run()
        {
            if (Ai.isDead)
                stateMachine.TransitionTo<DeadState>();
            
            if (Ai.waypoints.Length > 0)
            {
                Ai.agent.SetDestination(Ai.waypoints[currentPoint].position);
                if (Vector3.Distance(Ai.transform.position, Ai.waypoints[currentPoint].position) < 1)
                    currentPoint = (currentPoint + 1) % Ai.waypoints.Length;
            }
            else
            {
                Ai.agent.SetDestination(Ai.target.transform.position);
            }

            if ((CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < seeDistance) || Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < hearDistance)
            {
                stateMachine.TransitionTo<HuntState>();   
            }
        }

        private void ChooseClosest()
        {
            if (Ai.waypoints.Length > 0){
                int closest = 0;
                for (int i = 0; i < Ai.waypoints.Length; i++)
                {
                    float dist = Vector3.Distance(Ai.transform.position, Ai.waypoints[i].position);
                    if (dist < Vector3.Distance(Ai.transform.position, Ai.waypoints[closest].position))
                        closest = i;
                }
                currentPoint = closest;
            }
        }
    }
}

