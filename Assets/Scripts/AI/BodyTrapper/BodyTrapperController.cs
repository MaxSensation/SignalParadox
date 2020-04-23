using System;
using System.Collections;
using PlayerController;
using UnityEngine;

namespace AI.BodyTrapper
{
    public class BodyTrapperController : AIController
    {
        private bool isStuckOnPlayer;
        private void Start()
        {
            PlayerTrapable.onPlayerTrapped += StuckOnPlayer;
        }

        private void StuckOnPlayer(GameObject bodyTrapper)
        {
            if (bodyTrapper == gameObject)
            {
                GetComponent<SphereCollider>().enabled = false;
                isStuckOnPlayer = true;
                agent.enabled = false;
                rigidbody.velocity = Vector3.zero;
                rigidbody.useGravity = false;
            }
        }

        protected internal override void Die()
        {
            
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
            Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out var hit, 1f);
            if (hit.collider && hit.collider.CompareTag("Player"))
                onTrappedPlayer?.Invoke(gameObject);
        }
    }
}
