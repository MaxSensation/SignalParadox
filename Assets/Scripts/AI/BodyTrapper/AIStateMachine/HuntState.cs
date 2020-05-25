﻿//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/HuntState")]
    public class HuntState : BodyTrapperBaseState
    {
        [SerializeField] private float jumpDistance;
        [SerializeField] private float maxMinLookRange;

        public static Action<GameObject> onHuntEvent; 

        public override void Enter()
        {
            base.Enter();
            onHuntEvent?.Invoke(Ai.gameObject);
        }

        public override void Run()
        {
            if (!Ai.IsStunned && Ai.agent.enabled)
                Ai.agent.SetDestination(Ai.target.transform.position);
            
            if (Ai.isPlayerAlive && CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < jumpDistance && Ai.LookingAtPlayer(Ai, maxMinLookRange))
                stateMachine.TransitionTo<ChargeState>();

            if (CanSeePlayer() && Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < jumpDistance)
            {
                var directionToPlayer = (Ai.target.transform.position - Ai.transform.position);
                var targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z), Vector3.up);
                Ai.transform.rotation = Quaternion.Lerp(Ai.transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
}