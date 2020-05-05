//Main author: Maximiliam Rosén

using AI.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/StunState")]
    public class StunState : BodyTrapperBaseState
    {
        public override void Enter()
        {
            base.Enter();
            Ai.rigidbody.velocity = Vector3.zero;
            Ai.GetComponent<SphereCollider>().enabled = false;
            Ai.agent.enabled = false;
            Ai.rigidbody.useGravity = false;
            Ai.ActivateStun();
        }

        public override void Run()
        {
            Ai.rigidbody.velocity = Vector3.zero;
            if (!Ai._stunned)
            {
                stateMachine.TransitionTo<PatrolState>();
            }
        }

        public override void Exit()
        {
            base.Exit();
            Ai.GetComponent<SphereCollider>().enabled = true;
            Ai.rigidbody.useGravity = true;
            Ai.agent.enabled = true;
        }
    }
}