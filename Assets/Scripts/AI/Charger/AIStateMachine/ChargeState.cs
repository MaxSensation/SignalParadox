using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/ChargeState")]
    public class ChargeState : AiBaseState
    {
        [SerializeField] private float chargeSpeed;
        private Vector3 _chargeDirection;
        public override void Enter()
        {
            base.Enter();
            Ai.agent.enabled = false;
            GetChargeDirection();
        }

        private void GetChargeDirection()
        {
            _chargeDirection = Ai.target.transform.position - Ai.transform.position;
        }

        public override void Run()
        {
            if(!Ai.IsStunned())//.
            Charge();
            if (TouchingPlayer())
            {
                Ai.target.transform.parent = Ai.transform;
            }

            if (Ai.rigidbody.velocity.magnitude <= 0.001f)
            {
                if (Ai.target.transform.parent == Ai.transform)
                {
                    Ai.target.GetComponent<PlayerController>().Die();
                }
                Ai.target.transform.parent = null;
                Ai.agent.enabled = true;
                Ai.ActivateOnlyStun();
                stateMachine.TransitionTo<HuntState>();
            }
        }

        private bool TouchingPlayer()
        {
            return Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < 2f;
        }

        private void Charge()
        {
            Ai.rigidbody.AddForce(_chargeDirection.normalized * chargeSpeed);
        }
    }
}
