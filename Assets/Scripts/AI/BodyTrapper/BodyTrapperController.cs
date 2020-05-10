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
        [SerializeField][Tooltip("Tid tills bodytrapper går tillbaks till patrolState")] private float ignoreTime;
        [SerializeField] private AudioClip trappedPlayerSound;
        private EnemyTrigger _enemyTrigger;
        private float chargeTime;
        private bool _enemyWithinPlayerMelee;
        private Coroutine _foundSound;
        private Coroutine _ignorePlayer;
        private SphereCollider _bodyTrapperCollider;
        public static Action<GameObject> onTrappedPlayer;
        public static Action<GameObject> onDetachedFromPlayer;
        internal Vector3 lastSoundLocation;
        internal Vector3 jumpDirection;
        internal EchoLocationResult _echoLocationResult;
        internal EchoLocationReceiver _soundListener;
        internal NavMeshPath path;
        internal AudioSource audioSource;
        internal bool _hasHeardDecoy;
        internal bool _isPlayerAlive;
        internal bool canAttack;
        internal bool isStuckOnPlayer;
        internal bool isCharging;

        private new void Awake()
        {
            base.Awake();
            _isPlayerAlive = true;
            _bodyTrapperCollider = GetComponent<SphereCollider>();
            audioSource = GetComponent<AudioSource>();
            path = new NavMeshPath();
            chargeTime = 0f;
            _enemyTrigger = transform.Find("EnemyTrigger").GetComponent<EnemyTrigger>();
            PlayerTrapable.onTrapped += StuckOnPlayer;
            PlayerTrapable.onDetached += DetachFromPlayer;
            LaserController.onLaserDeath += OnDeathByTrap;
            SteamController.onSteamDamage += OnDeathByTrap;
            PlayerAnimatorController.OnDeathAnimBeginning += () => _isPlayerAlive = false;
            _soundListener = transform.GetComponentInChildren<EchoLocationReceiver>();
            _soundListener.heardSound += UpdateSoundSource;
            audioSource.Play();
        }

        private void UpdateSoundSource(EchoLocationResult echoLocationResult)
        {
            _echoLocationResult = echoLocationResult;
            if (echoLocationResult.Transmitter.CompareTag("Decoy"))
            {
                if (_ignorePlayer != null) StopCoroutine(_ignorePlayer);
                _ignorePlayer = StartCoroutine(IgnorePlayer());
                lastSoundLocation = echoLocationResult.Transmitter.transform.position;
                _hasHeardDecoy = true;
            }
            if (!_hasHeardDecoy && echoLocationResult.Transmitter.CompareTag("Player"))
                lastSoundLocation = echoLocationResult.Transmitter.transform.position;
            if (_foundSound != null) StopCoroutine(_foundSound);
            _foundSound = StartCoroutine(FoundSound());
        }

        private IEnumerator FoundSound()
        {
            yield return new WaitForSeconds(ignoreTime);
            lastSoundLocation = Vector3.zero;
        }

        private IEnumerator IgnorePlayer()
        {
            yield return new WaitForSeconds(0.5f);
            _hasHeardDecoy = false;
        }

        private void OnDeathByTrap(GameObject obj)
        {
            if (obj != gameObject) return;
            DetachFromPlayer();
            Die();
        }
        
        private void DetachFromPlayer()
        {
            if (!isStuckOnPlayer) return;
            onDetachedFromPlayer?.Invoke(gameObject);
            _bodyTrapperCollider.isTrigger = false;
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
            PlayerAnimatorController.OnDeathAnimBeginning += () => _isPlayerAlive = false;
        }

        private void StuckOnPlayer(GameObject bodyTrapper)
        {
            if (bodyTrapper != gameObject || isDead) return;
            _bodyTrapperCollider.isTrigger = true;
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
            audioSource.Stop();
            UnregisterEvents();
            _stateMachine.TransitionTo<DeadState>();
        }
        
        internal void TouchingPlayer()
        {
            if (!_enemyTrigger.IsTouchingTaggedObject || isStuckOnPlayer || !canAttack) return;
            canAttack = false;
            audioSource.PlayOneShot(trappedPlayerSound);
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
