using System;
using System.Collections;
using PlayerController;
using UnityEngine;

namespace AI.BodyTrapper
{
    public class BodyTrapperController : AIController
    {
        private bool isStuckOnPlayer;
        private EnemyTrigger _enemyTrigger;
        internal bool isCharging;
        private float chargeTime;
        internal Vector3 jumpDirection;

        private new void Awake()
        {
            base.Awake();
            chargeTime = 0f;
            _enemyTrigger = transform.Find("EnemyTrigger").GetComponent<EnemyTrigger>();
            PlayerTrapable.onPlayerTrapped += StuckOnPlayer;
        }

        private void OnDestroy()
        {
            PlayerTrapable.onPlayerTrapped -= StuckOnPlayer;
        }

        private void StuckOnPlayer(GameObject bodyTrapper)
        {
            if (bodyTrapper == gameObject)
            {
                GetComponent<SphereCollider>().enabled = false;
                isStuckOnPlayer = true;
                agent.enabled = false;
                rigidbody.velocity = Vector3.zero;
                rigidbody.useGravity = false;
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
            if (_enemyTrigger.IsTouchingEnemy)
                onTrappedPlayer?.Invoke(gameObject);
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
