//Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

using System;
using System.Collections;
using AI.Charger.AIStateMachine;
using Interactables.Traps;
using Interactables.Triggers;
using UnityEngine;

namespace AI.Charger
{
    public class ChargerController : AIController
    {
        [SerializeField] private int chargeUpTime = 1;
        private Coroutine onlyStunTime, chargeTime;
        private WaitForSeconds chargeUpTimeSeconds;

        internal Vector3 ChargeDirection { get; private set; }
        internal AudioSource AudioSource { get; private set; }
        internal EnemyTrigger EnemyTrigger { get; private set; }
        public static Action onCrushedPlayerEvent, onCaughtPlayerEvent;

        private new void Awake()
        {
            base.Awake();
            chargeUpTimeSeconds = new WaitForSeconds(chargeUpTime);
            EnemyTrigger = transform.Find("EnemyTrigger").GetComponent<EnemyTrigger>();
            AudioSource = GetComponent<AudioSource>();
            LaserController.onLaserDeath += OnDeathByLaser;
        }

        private void OnDestroy() => LaserController.onLaserDeath -= OnDeathByLaser;

        private void OnDeathByLaser(GameObject entity)
        {
            if (entity.Equals(gameObject))
                Die();
        }
        
        internal void KillPlayer() => onCrushedPlayerEvent?.Invoke();

        internal void CaughtPlayer() => onCaughtPlayerEvent?.Invoke();

        private IEnumerator ChargeTime()
        {
            yield return chargeUpTimeSeconds;
            if (agent.enabled)
                agent.isStopped = false;
            stateMachine.TransitionTo<ChargeState>();
        }

        internal void ChargeUp()
        {
            agent.isStopped = true;
            StartCoroutine(ChargeTime());
        }

        internal void SetChargeDirection()
        {
            var direction = (target.transform.position - transform.position).normalized;
            direction.y = 0;
            ChargeDirection = direction;
        }

        protected override void Die()
        {
            if (agent != null)
                agent.enabled = false;
            AudioSource.Stop();
            stateMachine.TransitionTo<DeadState>();
        }
    }
}
