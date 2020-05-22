//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using AI.BodyTrapper;
using UnityEngine;

namespace AI.AIStateMachine
{
    public abstract class BodyTrapperBaseState : State
    {
        private BodyTrapperController _ai;
        protected BodyTrapperController Ai => _ai = _ai ? _ai : (BodyTrapperController)owner;
        [SerializeField] protected Material material;

        public override void Enter()
        {
            //_renderer.material = material;
        }

        protected bool CanSeePlayer()
        {
            return !Physics.Linecast(Ai.transform.position, Ai.target.transform.position, Ai.visionMask);
        }
    }
}

