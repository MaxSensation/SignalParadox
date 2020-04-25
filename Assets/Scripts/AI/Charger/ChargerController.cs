using System;
using System.Collections;
using AI.Charger.AIStateMachine;
using UnityEngine;

namespace AI.Charger
{
    public class ChargerController : AIController
    {
        public static Action onCrushedPlayerEvent;
        public static Action CaughtPlayerEvent;
        private bool hasChargedUp;
        private BoxCollider _hitCollider;
        internal bool charging;
        private Vector3 _chargeDirection;
        private EnemyTrigger _enemyTrigger;
        [SerializeField] private int _chargeUpTime;

        private new void Awake()
        {
            base.Awake();
            _hitCollider = GetComponent<BoxCollider>();
            _enemyTrigger = transform.Find("EnemyTrigger").GetComponent<EnemyTrigger>();
        }

        private IEnumerator OnlyStunTime()
        {
            yield return new WaitForSeconds(3);
            _stunned = false;
        }

        private IEnumerator ChargeTime()
        {
            yield return new WaitForSeconds(_chargeUpTime);
            if (agent.enabled)
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

        internal void SetChargeDirection()
        {
            Vector3 enemyPosition = transform.position;
            Vector3 playerPosition = target.transform.position;
            _chargeDirection = (new Vector3(playerPosition.x, 0, playerPosition.z) - new Vector3(enemyPosition.x, 0, enemyPosition.z)).normalized;
        }

        internal Vector3 GetChargeDirection()
        {
            return _chargeDirection;
        }

        internal bool GetHasCollidedWithColliders()
        {
            return _enemyTrigger.IsTouchingLayerObject;
        }

        internal bool GetHasCollidedWithTaggedObjet()
        {
            return _enemyTrigger.IsTouchingTaggedObject;
        }

        internal void KillPlayer()
        {
            onCrushedPlayerEvent?.Invoke();
        }

        internal void CaughtPlayer()
        {
            CaughtPlayerEvent?.Invoke();
        }

        protected internal override void Die()
        {

        }

    }
}
