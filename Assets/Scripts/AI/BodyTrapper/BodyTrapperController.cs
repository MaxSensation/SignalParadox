//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using System.Collections;
using AI.BodyTrapper.AIStateMachine;
using EchoLocation;
using Interactables.Traps;
using Interactables.Triggers;
using PlayerController;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace AI.BodyTrapper
{
    public class BodyTrapperController : AIController
    {
        public UnityEvent onDeathEvent;
        internal bool isStuckOnPlayer;
        private EnemyTrigger _enemyTrigger;
        internal bool isCharging;
        private float chargeTime;
        internal Vector3 jumpDirection;
        internal bool canAttack;
        private bool _enemyWithinPlayerMelee;
        internal NavMeshPath path;
        internal EchoLocationReceiver _soundListener;
        internal Vector3 lastSoundLocation;
        private Coroutine _foundSound;
        private SphereCollider _collider;
        public static Action<GameObject> onTrappedPlayer;
        public static Action<GameObject> onDetachedFromPlayer;
        private new void Awake()
        {
            base.Awake();
            _collider = GetComponent<SphereCollider>();
            path = new NavMeshPath();
            chargeTime = 0f;
            _enemyTrigger = transform.Find("EnemyTrigger").GetComponent<EnemyTrigger>();
            PlayerTrapable.onTrapped += StuckOnPlayer;
            PlayerTrapable.onDetached += DetachFromPlayer;
            LaserController.onLaserDeath += OnDeathByTrap;
            SteamController.onSteamDamage += OnDeathByTrap;
            _soundListener = transform.GetComponentInChildren<EchoLocationReceiver>();
            _soundListener.heardSound += UpdateSoundSource;
        }

        private void UpdateSoundSource(EchoLocationResult echoLocationResult)
        {
            lastSoundLocation = echoLocationResult.Transmitter.transform.position;
            if (_foundSound != null) StopCoroutine(_foundSound);
            _foundSound = StartCoroutine(FoundSound());
        }

        private IEnumerator FoundSound()
        {
            yield return new WaitForSeconds(15f);
            lastSoundLocation = Vector3.zero;
        }

        private void OnDeathByTrap(GameObject obj)
        {
            if (obj == gameObject)
            {
                DetachFromPlayer();
                Die();
            }
        }
        
        private void DetachFromPlayer()
        {
            if (!isStuckOnPlayer) return;
            onDetachedFromPlayer?.Invoke(gameObject);
            _collider.isTrigger = false;
            isStuckOnPlayer = false;
            agent.enabled = true;
            rigidbody.useGravity = true;
            transform.parent = null;
            _stateMachine.TransitionTo<StunState>();
        }

        private void OnDestroy()
        {
            UnregisterEvents();
        }

        private void UnregisterEvents()
        {
            PlayerTrapable.onTrapped -= StuckOnPlayer;
            PlayerTrapable.onDetached -= DetachFromPlayer;
            LaserController.onLaserDeath -= OnDeathByTrap;
            SteamController.onSteamDamage -= OnDeathByTrap;
        }

        private void StuckOnPlayer(GameObject bodyTrapper)
        {
            if (bodyTrapper != gameObject || isDead) return;
            _collider.isTrigger = true;
            rigidbody.velocity = Vector3.zero;
            isStuckOnPlayer = true;
            agent.enabled = false;
            rigidbody.useGravity = false;
        }

        protected internal override void Die()
        {
            isDead = true;
            if (agent != null)
                agent.enabled = false;
            onDeathEvent?.Invoke();
            UnregisterEvents();
            _stateMachine.TransitionTo<DeadState>();
        }
        
        internal void TouchingPlayer()
        {
            if (_enemyTrigger.IsTouchingTaggedObject && !isStuckOnPlayer && canAttack)
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

        public void ActivateStun()
        {
            _stunned = true;
            StartCoroutine(Stun());
        }

        private IEnumerator Stun()
        {
            yield return new WaitForSeconds(5f);
            _stunned = false;
            agent.enabled = true;
        }
    }
}
