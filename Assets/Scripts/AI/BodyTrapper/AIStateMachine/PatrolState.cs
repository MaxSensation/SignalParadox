//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using AI.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/PatrolState")]
    public class PatrolState : BodyTrapperBaseState
    {
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
                if(Ai.agent.enabled)
                    Ai.agent.SetDestination(Ai.waypoints[currentPoint].position);
                if (Vector3.Distance(Ai.transform.position, Ai.waypoints[currentPoint].position) < 1)
                    currentPoint = (currentPoint + 1) % Ai.waypoints.Length;
            }
            else
            {
                Ai.agent.enabled = true;
                Ai.agent.isStopped = false;
                Ai.agent.SetDestination(Ai.target.transform.position);
            }

            if (Ai.lastSoundLocation != Vector3.zero)
            {
                    
                //NavMesh.CalculatePath(Ai.target.transform.position, Ai.lastSoundLocation, NavMesh.AllAreas, Ai.path);
                //if (Ai.path.status == NavMeshPathStatus.PathComplete)
                //{
                stateMachine.TransitionTo<SeekingState>();
                //}
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

