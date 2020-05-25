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
        private Vector3 chargeDirection;
        private EnemyTrigger enemyTrigger;
        private Coroutine onlyStunTime, chargeTime;
        private WaitForSeconds chargeUpTimeSeconds;

        internal AudioSource AudioSource { get; private set; }
        public static Action onCrushedPlayerEvent, CaughtPlayerEvent;

        private new void Awake()
        {
            base.Awake();
            chargeUpTimeSeconds = new WaitForSeconds(chargeUpTime);
            enemyTrigger = transform.Find("EnemyTrigger").GetComponent<EnemyTrigger>();
            AudioSource = GetComponent<AudioSource>();
            AudioSource.Play();
            LaserController.onLaserDeath += OnDeathByLaser;
        }

        private void OnDestroy() => LaserController.onLaserDeath -= OnDeathByLaser;

        private void OnDeathByLaser(GameObject obj)
        {
            if (obj.Equals(gameObject))
                Die();
        }

        private IEnumerator ChargeTime()
        {
            yield return chargeUpTimeSeconds;
            if (agent.enabled)
                agent.isStopped = false;
            stateMachine.TransitionTo<AIStateMachine.ChargeState>();
            StopCoroutine(chargeTime);
        }

        internal void ChargeUp()
        {
            agent.isStopped = true;
            chargeTime = StartCoroutine(ChargeTime());
        }

        internal void SetChargeDirection()
        {
            Vector3 enemyPosition = transform.position;
            Vector3 playerPosition = target.transform.position;
            chargeDirection = (new Vector3(playerPosition.x, 0, playerPosition.z) - new Vector3(enemyPosition.x, 0, enemyPosition.z)).normalized;
        }

        internal Vector3 GetChargeDirection()
        {
            return chargeDirection;
        }

        internal bool HasCollidedWithTaggedObjet()
        {
            return enemyTrigger.IsTouchingTaggedObject;
        }

        internal void KillPlayer()
        {
            onCrushedPlayerEvent?.Invoke();
        }

        internal void CaughtPlayer()
        {
            CaughtPlayerEvent?.Invoke();
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
