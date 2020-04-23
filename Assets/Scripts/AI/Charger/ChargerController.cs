using System;
using System.Collections;
using AI.Charger.AIStateMachine;
using UnityEngine;

namespace AI.Charger
{
    public class ChargerController : AIController
    {
        private bool hasChargedUp;
        public static Action onCrushedPlayer;
        private BoxCollider _hitCollider;

        private void Awake()
        {
            _hitCollider = GetComponent<BoxCollider>();
        }

        public void PlayerCrushed()
        {
            if (rigidbody.velocity.magnitude <= 0.001f)
            {
                if (target.transform.parent == transform)
                    onCrushedPlayer?.Invoke();
                target.transform.parent = null;
                agent.enabled = true;
                ActivateOnlyStun();
                _stateMachine.TransitionTo<HuntState>();
            }
        }

        private IEnumerator OnlyStunTime()
        {
            yield return new WaitForSeconds(3);
            _stunned = false;
        }

        private IEnumerator ChargeTime()
        {
            yield return new WaitForSeconds(2);
            agent.isStopped = false;
            hasChargedUp = true;
            //StopCoroutine("ChargeTime");
        }
        internal void ChargeUp()
        {
            agent.isStopped = true;
            hasChargedUp = false;
            StartCoroutine("ChargeTime");
        }

        internal void ActivateOnlyStun()
        {
            _stunned = true;
            StartCoroutine("OnlyStunTime");
        }

        internal bool GetHasChargedUp()
        {
            return hasChargedUp;
        }

        protected override void Die()
        {

        }

    }
}
