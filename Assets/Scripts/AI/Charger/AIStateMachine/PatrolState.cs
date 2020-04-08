using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/PatrolState")]
    public class PatrolState : AiBaseState
    {
        [SerializeField] private float seeDistance;

        private int currentPoint;
        
        public override void Enter()
        {
            base.Enter();
            ChooseClosest();
        }

        public override void Run()
        {
            if (Ai.waypoints.Length > 0)
            {
                Ai.agent.SetDestination(Ai.waypoints[currentPoint].position);
                if (Vector3.Distance(Ai.transform.position, Ai.waypoints[currentPoint].position) < 1)
                    currentPoint = (currentPoint + 1) % Ai.waypoints.Length;
            }

            if ((CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < seeDistance))
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

