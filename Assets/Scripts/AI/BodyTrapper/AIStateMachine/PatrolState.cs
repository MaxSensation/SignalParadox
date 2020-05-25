//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using UnityEngine;
 
namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/PatrolState")]
    public class PatrolState : BodyTrapperBaseState
    {
        private int currentWaypoint;

        public override void Enter()
        {
            base.Enter();
            SetClosestWaypoint();
        }

        public override void Run()
        {
            if (Ai.waypoints.Length > 0)
            {
                if (Ai.agent.enabled)
                    Ai.agent.SetDestination(Ai.waypoints[currentWaypoint].position);
                if (Vector3.Distance(Ai.transform.position, Ai.waypoints[currentWaypoint].position) < 1)
                    currentWaypoint = (currentWaypoint + 1) % Ai.waypoints.Length;
            }
            else
                throw new MissingFieldException("BodyTrapper missing waypoints! Please add waypoints to Bodytrapper");

            if (Ai.lastSoundLocation != Vector3.zero)
                stateMachine.TransitionTo<SeekingState>();
        }

        private void SetClosestWaypoint()
        {
            if (Ai.waypoints.Length <= 0) return;
            var closestWayPoint = 0;
            for (var i = 0; i < Ai.waypoints.Length; i++)
            {
                var distanceToWaypoint = Vector3.Distance(Ai.transform.position, Ai.waypoints[i].position);
                if (distanceToWaypoint < Vector3.Distance(Ai.transform.position, Ai.waypoints[closestWayPoint].position))
                    closestWayPoint = i;
            }
            currentWaypoint = closestWayPoint;
        }
    }
}

