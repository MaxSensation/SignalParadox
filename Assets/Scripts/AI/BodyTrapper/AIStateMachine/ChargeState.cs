using AI.AIStateMachine;
using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/BodyTrapper/ChargeState")]
    public class ChargeState : BodyTrapperBaseState
    {
        [SerializeField] private float chargeTime;


        public override void Enter()
        {
            base.Enter();
            Ai.StartCharge(chargeTime);
            Ai.agent.isStopped = true;
            var enemyPosition = Ai.transform.position;
            var playerPosition = Ai.target.transform.position;
            Ai.jumpDirection = (new Vector3(playerPosition.x, 0, playerPosition.z)  - new Vector3(enemyPosition.x, 0, enemyPosition.z)).normalized;
        }

        public override void Run()
        {
            if (!Ai.isCharging)
            {
                stateMachine.TransitionTo<JumpState>();
            }
        }
    }
}