using System;
using System.Collections;
using PlayerController;
using UnityEngine;

namespace AI.BodyTrapper
{
    public class BodyTrapperController : AIController
    {
        internal bool isStuckOnPlayer;
        private EnemyTrigger _enemyTrigger;
        internal bool isCharging;
        private float chargeTime;
        internal Vector3 jumpDirection;
        internal bool canAttack;

        private new void Awake()
        {
            base.Awake();
            chargeTime = 0f;
            _enemyTrigger = transform.Find("EnemyTrigger").GetComponent<EnemyTrigger>();
            PlayerTrapable.onTrapped += StuckOnPlayer;
            PlayerTrapable.onDetached += DetachFromPlayer;
        }

        private void DetachFromPlayer()
        {
            GetComponent<SphereCollider>().enabled = true;
            isStuckOnPlayer = false;
            agent.enabled = true;
            rigidbody.useGravity = true;
            transform.parent = null;
            _stunned = false;
        }

        private void OnDestroy()
        {
            PlayerTrapable.onTrapped -= StuckOnPlayer;
            PlayerTrapable.onDetached -= DetachFromPlayer;
        }

        private void StuckOnPlayer(GameObject bodyTrapper)
        {
            if (bodyTrapper == gameObject)
            {
                GetComponent<SphereCollider>().enabled = false;
                isStuckOnPlayer = true;
                agent.enabled = false;
                rigidbody.useGravity = false;
                rigidbody.velocity = Vector3.zero;
            }
        }

        protected internal override void Die()
        {
            isDead = true;
        }

        public static Action<GameObject> onTrappedPlayer;

        public void ActivateStun()
        {
            agent.enabled = false;
            _stunned = true;
            StartCoroutine("StunTime");
        }
        private IEnumerator StunTime()
        {
            yield return new WaitForSeconds(3);
            if (!isStuckOnPlayer)
            {
                agent.enabled = true;
                _stunned = false;
            }
        }
        internal void TouchingPlayer()
        {
            if (_enemyTrigger.IsTouchingObject && !isStuckOnPlayer && canAttack)
            {
                canAttack = false;
                onTrappedPlayer?.Invoke(gameObject);
            }
        }

        public void StartCharge(float chargeTime)
        {
            this.chargeTime = chargeTime;
            isCharging = true;
            StartCoroutine("ChargeTime");
        }

        private IEnumerator ChargeTime()
        {
            yield return new WaitForSeconds(chargeTime);
            isCharging = false;
        }
    }
}
