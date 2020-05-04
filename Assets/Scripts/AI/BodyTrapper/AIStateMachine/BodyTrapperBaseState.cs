//Main author: Maximiliam Rosén

using AI.BodyTrapper;
using UnityEngine;

namespace AI.AIStateMachine
{
    public abstract class BodyTrapperBaseState : State
    {
        private BodyTrapperController _ai;
        protected BodyTrapperController Ai => _ai = _ai ? _ai : (BodyTrapperController)owner;
        protected Renderer _renderer => Ai.GetRenderer();
        [SerializeField] protected Material material;

        public override void Enter()
        { 
            _renderer.material = material;
        }

        protected bool CanSeePlayer()
        {
            return !Physics.Linecast(Ai.transform.position, Ai.target.transform.position, Ai.visionMask);
        }
    }
}

