//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using System.Collections;
using AI.BodyTrapper.AIStateMachine;
using EchoLocation;
using Interactables.Traps;
using Interactables.Triggers;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace AI.BodyTrapper
{
    public class BodyTrapperController : AIController
    {
        [SerializeField][Tooltip("Tid tills bodytrapper går tillbaks till patrolState")] private float ignoreTime;
        [SerializeField] private AudioClip trappedPlayerSound;
        private EnemyTrigger enemyTrigger;
        private float chargeTime;
        private Coroutine foundSound, ignorePlayer;
        private SphereCollider bodyTrapperCollider;
        public static Action<GameObject> onTrappedPlayer, onDetachedFromPlayer;
        internal Vector3 lastSoundLocation, jumpDirection;
        internal EchoLocationResult echoLocationResult;
        internal EchoLocationReceiver soundListener;
        internal NavMeshPath path;
        internal AudioSource audioSource;
        internal bool hasHeardDecoy, isPlayerAlive ,canAttack ,isStuckOnPlayer ,isCharging;

        private new void Awake()
        {
            base.Awake();
            isPlayerAlive = true;
            bodyTrapperCollider = GetComponent<SphereCollider>();
            audioSource = GetComponent<AudioSource>();
            path = new NavMeshPath();
            chargeTime = 0f;
            enemyTrigger = transform.Find("EnemyTrigger").GetComponent<EnemyTrigger>();
            soundListener = transform.GetComponentInChildren<EchoLocationReceiver>();
            soundListener.heardSound += UpdateSoundSource;
            audioSource.Play();
            PlayerTrapable.onTrapped += StuckOnPlayer;
            PlayerTrapable.onDetached += DetachFromPlayer;
            LaserController.onLaserDeath += OnDeathByTrap;
            SteamController.onSteamDamage += OnDeathByTrap;
            PlayerAnimatorController.OnDeathAnimBeginning += () => isPlayerAlive = false;
        }

        private void UpdateSoundSource(EchoLocationResult echoLocationResult)
        {
            this.echoLocationResult = echoLocationResult;
            if (echoLocationResult.Transmitter.CompareTag("Decoy"))
            {
                if (ignorePlayer != null) StopCoroutine(ignorePlayer);
                ignorePlayer = StartCoroutine(IgnorePlayer());
                lastSoundLocation = echoLocationResult.Transmitter.transform.position;
                hasHeardDecoy = true;
            }
            if (!hasHeardDecoy && echoLocationResult.Transmitter.CompareTag("Player"))
                lastSoundLocation = echoLocationResult.Transmitter.transform.position;
            if (foundSound != null) StopCoroutine(foundSound);
            foundSound = StartCoroutine(FoundSound());
        }

        private IEnumerator FoundSound()
        {
            yield return new WaitForSeconds(ignoreTime);
            lastSoundLocation = Vector3.zero;
        }

        private IEnumerator IgnorePlayer()
        {
            yield return new WaitForSeconds(0.5f);
            hasHeardDecoy = false;
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
            bodyTrapperCollider.isTrigger = false;
            isStuckOnPlayer = false;
            agent.enabled = true;
            aiRigidbody.useGravity = true;
            transform.parent = null;
            stateMachine.TransitionTo<StunState>();
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
            PlayerAnimatorController.OnDeathAnimBeginning += () => isPlayerAlive = false;
        }

        private void StuckOnPlayer(GameObject bodyTrapper)
        {
            if (bodyTrapper != gameObject || isDead) return;
            bodyTrapperCollider.isTrigger = true;
            aiRigidbody.velocity = Vector3.zero;
            isStuckOnPlayer = true;
            agent.enabled = false;
            aiRigidbody.useGravity = false;
        }

        protected override void Die()
        {
            isDead = true;
            if (agent != null)
                agent.enabled = false;
            audioSource.Stop();
            UnregisterEvents();
            stateMachine.TransitionTo<DeadState>();
        }
        
        internal void TouchingPlayer()
        {
            if (!enemyTrigger.IsTouchingTaggedObject || isStuckOnPlayer || !canAttack) return;
            canAttack = false;
            audioSource.PlayOneShot(trappedPlayerSound);
            onTrappedPlayer?.Invoke(gameObject);
        }

        internal void StartCharge(float chargeTime)
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
