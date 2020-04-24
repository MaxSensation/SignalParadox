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
        internal bool charging;
        private Vector3 _chargeDirection;
        [SerializeField] private int _chargeUpTime;


        private new void Awake()
        {
            base.Awake();
            _hitCollider = GetComponent<BoxCollider>();
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
            //Ai.agent.enabled = false;
            //    var enemyPosition = Ai.transform.position;
            //var playerPosition = Ai.target.transform.position;
            //Ai.jumpDirection = (new Vector3(playerPosition.x, 0, playerPosition.z)  - new Vector3(enemyPosition.x, 0, enemyPosition.z)).normalized;
            Vector3 enemyPosition = transform.position;
            Vector3 playerPosition = target.transform.position;
            _chargeDirection = (new Vector3(playerPosition.x, 0, playerPosition.z) - new Vector3(enemyPosition.x, 0, enemyPosition.z)).normalized;
        }

        internal Vector3 GetChargeDirection()
        {
            return _chargeDirection;
        }

        internal void KillPlayer()
        {
            onCrushedPlayer?.Invoke();
        }

        protected internal override void Die()
        {

        }

    }
}
