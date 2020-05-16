//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    [CreateAssetMenu(menuName = "AIStates/Charger/ChargeState")]
    public class ChargeState : ChargerBaseState
    {
        [SerializeField] private float chargeSpeed;
        [SerializeField] private AudioClip hitWallSound;
        private Vector3 _chargeDirection;
        private float previousFrameSpeed;
        public override void Enter()
        {
            base.Enter();
            Ai.charging = true;
        }

        public override void Run()
        {

            if (!Ai.isDead)
            {
                if ((previousFrameSpeed - Ai.rigidbody.velocity.magnitude) > 0f)
                    ChargeEnded();

                if (!Ai.IsStunned())
                    Charge();
            }
            else
                stateMachine.TransitionTo<DeadState>();
        }

        private void Charge()
        {
            previousFrameSpeed = Ai.rigidbody.velocity.magnitude;
            _chargeDirection = Ai.GetChargeDirection();
            Ai.rigidbody.AddForce(_chargeDirection.normalized * chargeSpeed * Time.deltaTime);
            if (Ai.HasCollidedWithTaggedObjet())
            {
                Ai.target.transform.parent = Ai.transform;
                Ai.CaughtPlayer();
            }
        }

        public void ChargeEnded()
        {
            if ((previousFrameSpeed - Ai.rigidbody.velocity.magnitude) > 10f)
            {
                Ai.audioSource.PlayOneShot(hitWallSound);
                Ai.rigidbody.velocity = Vector3.zero;
            }
            previousFrameSpeed = 0f;
            if (Ai.target.transform.IsChildOf(Ai.transform))
                Ai.KillPlayer();
            Ai.target.transform.parent = null;
            Ai.agent.enabled = true;
            Ai.ActivateStun();
            stateMachine.TransitionTo<HuntState>();
        }
    }
}
