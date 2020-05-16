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
        private float previousFrameSpeed;
        private bool hasCaughtPlayer;

        public override void Enter()
        {
            base.Enter();
            Ai.charging = true;
        }

        public override void Run()
        {
            //Debug.Log(previousFrameSpeed - Ai.rigidbody.velocity.magnitude);

            if (!Ai.isDead)
            {
                if ((previousFrameSpeed - Ai.rigidbody.velocity.magnitude) > 1f)
                    ChargeEnded();

                if (!Ai.IsStunned())
                    Charge();

                if (Ai.HasCollidedWithTaggedObjet())
                {
                    Debug.Log("bruh" + hasCaughtPlayer);
                    Ai.target.transform.parent = Ai.transform;
                    Ai.CaughtPlayer();
                    hasCaughtPlayer = true;
                }

            }
            else
                stateMachine.TransitionTo<DeadState>();
        }

        private bool TouchingPlayer()
        {
            return Vector3.Distance(Ai.transform.position, Ai.target.transform.position) < 2f;
        }

        private void Charge()
        {
            previousFrameSpeed = Ai.rigidbody.velocity.magnitude;
            _chargeDirection = Ai.GetChargeDirection();
            Ai.rigidbody.AddForce(_chargeDirection.normalized * chargeSpeed * Time.deltaTime);
        }

        public void ChargeEnded()
        {
            if ((previousFrameSpeed - Ai.rigidbody.velocity.magnitude) > 10f)
                Ai.audioSource.PlayOneShot(hitWallSound);
            Ai.rigidbody.velocity = Vector3.zero;
            previousFrameSpeed = 0f;
            if (hasCaughtPlayer || Ai.target.transform.parent == Ai.transform || Ai.target.transform.IsChildOf(Ai.transform)) //Icke fungerade även fast spelaren är child till chargern
                Ai.KillPlayer();
            Ai.target.transform.parent = null;
            Ai.agent.enabled = true;
            Ai.ActivateStun();
            stateMachine.TransitionTo<HuntState>();
        }
    }
}
