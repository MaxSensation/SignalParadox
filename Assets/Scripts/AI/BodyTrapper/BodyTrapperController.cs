using System;
using System.Collections;
using UnityEngine;

namespace AI.BodyTrapper
{
    public class BodyTrapperController : AIController
    {
        public static Action<GameObject> onTrappedPlayer;
        public void ActivateStun()
        {
            agent.isStopped = false;
            _stunned = true;
            StartCoroutine("StunTime");
        }
        private IEnumerator StunTime()
        {
            yield return new WaitForSeconds(3);
            agent.isStopped = true;
            _stunned = false;
        }
        internal void TouchingPlayer()
        {
            Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out var hit, 1f);
            if (hit.collider && hit.collider.CompareTag("Player"))
                onTrappedPlayer?.Invoke(gameObject);
        }
    }
}
