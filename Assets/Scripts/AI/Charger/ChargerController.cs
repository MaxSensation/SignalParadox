//Main author: Andreas Berzelius
//Secondary author: Maximiliam Rosén

using System;
using System.Collections;
using Interactables.Traps;
using Interactables.Triggers;
using UnityEngine;

namespace AI.Charger
{
    public class ChargerController : AIController
    {
        [SerializeField] private int _chargeUpTime = 1;
        [SerializeField] private float _stunTime = 0.5f;
        public static Action onCrushedPlayerEvent;
        public static Action CaughtPlayerEvent;
        private bool hasChargedUp;
        internal bool charging;
        private Vector3 _chargeDirection;
        private EnemyTrigger _enemyTrigger;
        internal AudioSource audioSource;

        private new void Awake()
        {
            base.Awake();
            _enemyTrigger = transform.Find("EnemyTrigger").GetComponent<EnemyTrigger>();
            audioSource = GetComponent<AudioSource>();
            LaserController.onLaserDeath += OnDeathByLaser;
            audioSource.Play();
        }

        private void OnDestroy()
        {
            LaserController.onLaserDeath -= OnDeathByLaser;
        }

        private void OnDeathByLaser(GameObject obj)
        {
            if (obj.Equals(gameObject))
                Die();
        }

        private IEnumerator OnlyStunTime()
        {
            yield return new WaitForSeconds(_stunTime);
            _stunned = false;
        }

        private IEnumerator ChargeTime()
        {
            yield return new WaitForSeconds(_chargeUpTime);
            if (agent.enabled)
                agent.isStopped = false;
            hasChargedUp = true;
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

        internal bool HasCollidedWithLayerObject()
        {
            return _enemyTrigger.IsTouchingLayerObject;
        }

        internal bool CheckForWallRayCast()
        {
            LayerMask layerMask = LayerMask.GetMask("Colliders");
            Physics.Raycast(transform.localPosition + Vector3.up, transform.forward, out RaycastHit hit, 1f, layerMask);
            return hit.collider;
        }

        internal bool HasCollidedWithTaggedObjet()
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
            if (rigidbody.velocity.magnitude <= 0.001f)
                KillPlayer();
        }

        protected internal override void Die()
        {
            isDead = true;
            if (agent != null)
                agent.enabled = false;
            audioSource.Stop();
        }
    }
}
