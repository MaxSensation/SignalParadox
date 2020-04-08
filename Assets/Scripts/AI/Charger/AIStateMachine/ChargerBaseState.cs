using UnityEngine;

namespace AI.Charger.AIStateMachine
{
    public abstract class AiBaseState : State
    {
        private AIController _ai;
        protected AIController Ai => _ai = _ai ? _ai : (AIController)owner;
        protected Renderer _renderer => Ai.GetRenderer();
        protected CapsuleCollider AiCollider => Ai.GetCollider();
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

