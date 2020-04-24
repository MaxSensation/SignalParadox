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
            SphereOverlapp();
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

        private void SphereOverlapp()
        {
            var Point1 = (Ai._collider.height / 2) - Ai._collider.radius;
            Collider[] colliders = Physics.OverlapSphere(new Vector3(Ai.transform.position.x, Point1, Ai.transform.position.z), 1f, layerMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                Debug.Log(colliders[i].tag);
                if (colliders[i].CompareTag("Finish") || colliders[i].CompareTag("Player"))
                {
                    Ai.charging = false;
                }
                else
                    Ai.charging = true;
            }
        }

        public void PlayerCrushed()
        {
            if (Ai.rigidbody.velocity.magnitude <= 0.001f && !Ai.charging)
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
