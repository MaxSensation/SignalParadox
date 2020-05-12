//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/ChargeState")]
    public class ChargeState : ChargerBaseState
    {
        [SerializeField] private float chargeSpeed;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private AudioClip hitWallSound;

        private Vector3 _chargeDirection;
        public override void Enter()
        {
            base.Enter();
            Ai.charging = true;
        }

        public override void Run()
        {
            Ai.CheckForWallRayCast();
            if (Ai.isDead)
                stateMachine.TransitionTo<DeadState>();

            if (!Ai.IsStunned())
                Charge();
            if (Ai.HasCollidedWithTaggedObjet())
            {
                Ai.target.transform.parent = Ai.transform;
                Ai.CaughtPlayer();
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
            if (Ai.rigidbody.velocity.magnitude <= 0.001f && (Ai.HasCollidedWithLayerObject() || Ai.CheckForWallRayCast()))
            {
                if (Ai.target.transform.parent == Ai.transform)
                    Ai.KillPlayer();
                Ai.audioSource.PlayOneShot(hitWallSound);
                Ai.target.transform.parent = null;
                Ai.agent.enabled = true;
                Ai.ActivateOnlyStun();
                stateMachine.TransitionTo<HuntState>();
            }
        }

    }
}
