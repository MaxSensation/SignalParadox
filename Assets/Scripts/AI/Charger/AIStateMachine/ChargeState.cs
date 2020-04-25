using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/ChargeState")]
    public class ChargeState : ChargerBaseState
    {
        [SerializeField] private float chargeSpeed;
        [SerializeField] private LayerMask layerMask;
        private Vector3 _chargeDirection;
        public override void Enter()
        {
            base.Enter();
            Ai.charging = true;
            Debug.Log("Enterd Charge State");
        }

        public override void Run()
        {
            if (!Ai.IsStunned())
                Charge();
            if (TouchingPlayer())
            {
                Ai.target.transform.parent = Ai.transform;
            }
            PlayerCrushed();
        }

        private bool TouchingPlayer()
        {
            return Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < 2f;
        }

        private void Charge()
        {
            _chargeDirection = Ai.GetChargeDirection();
            Ai.rigidbody.AddForce(_chargeDirection.normalized * chargeSpeed);
        }

        public void PlayerCrushed()
        {
            if (Ai.rigidbody.velocity.magnitude <= 0.001f && Ai.GetHasCollidedWithColliders())
            {
                if (Ai.target.transform.parent == Ai.transform)
                    Ai.KillPlayer();
                Ai.target.transform.parent = null;
                Ai.agent.enabled = true;
                Ai.ActivateOnlyStun();
                stateMachine.TransitionTo<HuntState>();
            }
        }

    }
}
