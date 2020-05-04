//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;
namespace AI.Charger.AIStateMachine
{
    public abstract class ChargerBaseState : State
    {
        private ChargerController _ai;
        protected ChargerController Ai => _ai = _ai ? _ai : (ChargerController)owner;
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

