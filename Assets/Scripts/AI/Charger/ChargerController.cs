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
        [SerializeField] private float _stunnedTime = 0.5f;
        private bool hasChargedUp;
        private Vector3 _chargeDirection;
        private EnemyTrigger _enemyTrigger;
        private Coroutine onlyStunTime, chargeTime;
        private WaitForSeconds chargeUpTime, stunTime;

        internal bool charging;
        internal AudioSource audioSource;
        public static Action onCrushedPlayerEvent;
        public static Action CaughtPlayerEvent;

        private new void Awake()
        {
            base.Awake();
            chargeUpTime = new WaitForSeconds(_chargeUpTime);
            stunTime = new WaitForSeconds(_stunnedTime);
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

        private IEnumerator StunTime()
        {
            yield return stunTime;
            isStunned = false;
            StopCoroutine(onlyStunTime);
        }

        private IEnumerator ChargeTime()
        {
            yield return chargeUpTime;
            if (agent.enabled)
                agent.isStopped = false;
            hasChargedUp = true;
            StopCoroutine(chargeTime);
        }

        internal void ChargeUp()
        {
            agent.isStopped = true;
            hasChargedUp = false;
            chargeTime = StartCoroutine(ChargeTime());
        }

        internal void ActivateStun()
        {
            isStunned = true;
            onlyStunTime = StartCoroutine(StunTime());
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
        }

        protected override void Die()
        {
            isDead = true;
            if (agent != null)
                agent.enabled = false;
            audioSource.Stop();
        }


        //private void OnDrawGizmos()
        //{
        //    if (_collider != null)
        //    {
        //        var currentForward = transform.forward + transform.position;
        //        var capsulePosition = new Vector3(currentForward.x, currentForward.y, currentForward.z- 1f) + _collider.center;
        //        var distanceToPoints = (_collider.height / 2) - _collider.radius;
        //        var point1 = capsulePosition + Vector3.up * distanceToPoints;
        //        var point2 = capsulePosition + Vector3.down * distanceToPoints;
        //        Physics.CapsuleCast(point1, point2, _collider.radius, transform.forward.normalized, out var hit, 0.1f, visionMask);

        //        if (hit.collider)
        //        {
        //            Gizmos.color = Color.red;
        //            //Gizmos.DrawWireSphere(point1 + point2, hit.point.x);
        //            Gizmos.DrawRay(point1, hit.point);
        //        }
        //        else
        //            Gizmos.color = Color.green;
        //        Gizmos.DrawWireSphere(point1, _collider.radius);
        //        Gizmos.DrawWireSphere(point2, _collider.radius);
        //    }
        //}
    }
}
