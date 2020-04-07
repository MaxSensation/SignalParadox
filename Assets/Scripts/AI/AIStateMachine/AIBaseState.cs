using UnityEngine;

namespace AI.AIStateMachine
{
    public abstract class AiBaseState : State
    {
        private AIController _ai;
        protected AIController Ai => _ai = _ai ? _ai : (AIController)owner;

        protected bool CanSeePlayer()
        {
            return !Physics.Linecast(Ai.transform.position, Ai.target.transform.position, Ai.visionMask);
        }
    }
}

