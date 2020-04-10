﻿using AI.BodyTrapper.AIStateMachine;
using UnityEngine;
using PatrolState = AI.Charger.AIStateMachine.PatrolState;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/HuntState")]
    public class HuntState : AiBaseState
    {
        [SerializeField] private float chargeDistance;
        [SerializeField] private float searchingRange;
        public override void Run()
        {
            if (!Ai.IsStunned())
            {
                Ai.agent.SetDestination(Ai.target.transform.position);   
            }

            if (!Ai.IsStunned() && CanCharge() && CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < chargeDistance)
                stateMachine.TransitionTo<ChargeState>();
            
            if (!CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) > searchingRange)
                stateMachine.TransitionTo<PatrolState>();
            
        }

        private bool CanCharge()
        {
            var capsulePosition = (Ai.transform.position + AiCollider.center)/2;
            var distanceToPoints = (AiCollider.height / 2) - AiCollider.radius;
            var point1 = capsulePosition + Vector3.up * distanceToPoints;
            var point2 = capsulePosition + Vector3.down * distanceToPoints;
            Physics.CapsuleCast(point1, point2, AiCollider.radius, Ai.transform.forward.normalized, out var hit, (Ai.target.transform.position - Ai.transform.position).magnitude, Ai.visionMask);
            return !(hit.collider);
        }
    }
}