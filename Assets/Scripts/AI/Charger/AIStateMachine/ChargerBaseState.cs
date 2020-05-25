//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;
namespace AI.Charger.AIStateMachine
{
    public abstract class ChargerBaseState : State
    {
        private ChargerController ai;
        protected ChargerController Ai => ai = ai ? ai : (ChargerController)owner;
        protected CapsuleCollider AiCollider => Ai.AiCollider;
        [SerializeField] protected Material material;
        
        //public override void Enter() => _renderer.material = material;
    }
}

