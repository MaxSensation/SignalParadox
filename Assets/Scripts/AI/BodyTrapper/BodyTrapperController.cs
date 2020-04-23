using System.Collections;
using UnityEngine;

namespace AI.BodyTrapper
{
    public class BodyTrapperController : AIController
    {
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
    }
}
