using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorControllerStateMachine
{
    public abstract class DoorControllerBaseState : State
    {
        private DoorController _doorController;
        protected DoorController DoorController => _doorController = _doorController ? _doorController : (DoorController)owner;
    }
}
