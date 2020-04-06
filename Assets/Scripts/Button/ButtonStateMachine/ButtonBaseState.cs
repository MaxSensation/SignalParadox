using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ButtonStateMachine
{
    public abstract class ButtonBaseState : State
    {
        private ButtonController _button;
        protected ButtonController Button => _button = _button ? _button : (ButtonController)owner;
    }
}

