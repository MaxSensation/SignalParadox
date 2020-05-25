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

namespace AI.BodyTrapper
{
    public class BodyTrapperController : AIController
    {
        public static Action<GameObject> onTrappedPlayer, onDetachedFromPlayer;
        [SerializeField][Tooltip("Tid tills bodytrapper går tillbaks till patrolState")] private float seekTime;
        [SerializeField][Tooltip("Frekvens på hur ofta den checkar efter en Decoy")] private float ignorePlayerFrekvence;
        [SerializeField] private AudioClip trappedPlayerSound;
        internal Vector3 lastSoundLocation, jumpDirection;
        internal EchoLocationResult echoLocationResult;
        internal AudioSource audioSource;
        internal bool hasHeardDecoy, isPlayerAlive ,canAttack ,isStuckOnPlayer ,isCharging;
        private EnemyTrigger enemyTrigger;
        private float chargeTime;
        private Coroutine foundSound, ignorePlayer;
        private SphereCollider bodyTrapperCollider;
        private EchoLocationReceiver soundListener;
        private WaitForSeconds ignoreTimeSeconds, seekTimeSeconds;

        private new void Awake()
        {
            base.Awake();
            ignoreTimeSeconds = new WaitForSeconds(ignorePlayerFrekvence);
            seekTimeSeconds = new WaitForSeconds(seekTime);
            isPlayerAlive = true;
            bodyTrapperCollider = GetComponent<SphereCollider>();
            audioSource = GetComponent<AudioSource>();
            enemyTrigger = transform.Find("EnemyTrigger").GetComponent<EnemyTrigger>();
            soundListener = transform.GetComponentInChildren<EchoLocationReceiver>();
            
            soundListener.heardSound += UpdateSoundSource;
            PlayerTrapable.onTrapped += StuckOnPlayer;
            PlayerTrapable.onDetached += DetachFromPlayer;
            LaserController.onLaserDeath += OnDeathByTrap;
            SteamController.onSteamDamage += OnDeathByTrap;
            PlayerAnimatorController.OnDeathAnimBeginning += () => isPlayerAlive = false;
        }

        private void OnDestroy()
        {
            PlayerTrapable.onTrapped -= StuckOnPlayer;
            PlayerTrapable.onDetached -= DetachFromPlayer;
            LaserController.onLaserDeath -= OnDeathByTrap;
            SteamController.onSteamDamage -= OnDeathByTrap;
            PlayerAnimatorController.OnDeathAnimBeginning -= () => isPlayerAlive = false;
        }
        

        private void UpdateSoundSource(EchoLocationResult soundData)
        {
            echoLocationResult = soundData;
            if (soundData.Transmitter.CompareTag("Decoy"))
            {
                if (ignorePlayer != null) 
                    StopCoroutine(ignorePlayer);
                ignorePlayer = StartCoroutine(IgnorePlayer());
                lastSoundLocation = soundData.Transmitter.transform.position;
                hasHeardDecoy = true;
            }
            if (!hasHeardDecoy && soundData.Transmitter.CompareTag("Player"))
                lastSoundLocation = soundData.Transmitter.transform.position;
            if (foundSound != null) 
                StopCoroutine(foundSound);
            foundSound = StartCoroutine(FoundSound());
        }

        private IEnumerator FoundSound()
        {
            yield return seekTimeSeconds;
            lastSoundLocation = Vector3.zero;
        }

        private IEnumerator IgnorePlayer()
        {
            yield return ignoreTimeSeconds;
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
