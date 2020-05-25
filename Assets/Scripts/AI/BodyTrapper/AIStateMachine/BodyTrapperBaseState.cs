//Main author: Maximiliam Rosén
//Secondary author: Andreas Berzelius

using UnityEngine;

namespace AI.BodyTrapper.AIStateMachine
{
    public abstract class BodyTrapperBaseState : State
    {
        private BodyTrapperController ai;
        protected BodyTrapperController Ai => ai = ai ? ai : (BodyTrapperController)owner;
        [SerializeField] protected Material material;

        //public override void Enter() => _renderer.material = material;
    }
}

