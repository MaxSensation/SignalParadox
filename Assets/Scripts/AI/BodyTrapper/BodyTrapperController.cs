//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using System;
using System.Collections;
using EchoLocation;
using Interactables.Traps;
using Interactables.Triggers;
using PlayerController;
using UnityEngine;
using UnityEngine.AI;

namespace AI.BodyTrapper
{
    public class BodyTrapperController : AIController
    {
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
        private new void Awake()
        {
            base.Awake();
            path = new NavMeshPath();
            chargeTime = 0f;
            _enemyTrigger = transform.Find("EnemyTrigger").GetComponent<EnemyTrigger>();
            PlayerTrapable.onTrapped += StuckOnPlayer;
            PlayerTrapable.onDetached += DetachFromPlayer;
            LaserController.onLaserDeath += OnDeathByTrap;
            SteamController.onSteamDeath += OnDeathByTrap;
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
                Die();
        }
        
        private void DetachFromPlayer()
        {
            GetComponent<SphereCollider>().enabled = true;
            isStuckOnPlayer = false;
            agent.enabled = true;
            rigidbody.useGravity = true;
            transform.parent = null;
            _stunned = false;
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
            SteamController.onSteamDeath -= OnDeathByTrap;
        }

        private void StuckOnPlayer(GameObject bodyTrapper)
        {
            if (bodyTrapper == gameObject && !isDead)
            {
                GetComponent<SphereCollider>().enabled = false;
                isStuckOnPlayer = true;
                agent.enabled = false;
                rigidbody.useGravity = false;
                rigidbody.velocity = Vector3.zero;
            }
        }

        protected internal override void Die()
        {
            Debug.Log("bodytrapper is dead");
            isDead = true;
            if (agent != null)
                agent.enabled = false;
            UnregisterEvents();
        }

        public static Action<GameObject> onTrappedPlayer;

        public void ActivateStun()
        {
            agent.enabled = false;
            _stunned = true;
            StartCoroutine("StunTime");
        }
        private IEnumerator StunTime()
        {
            yield return new WaitForSeconds(3);
            if (!isStuckOnPlayer)
            {
                agent.enabled = true;
                _stunned = false;
            }
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
    }
}
