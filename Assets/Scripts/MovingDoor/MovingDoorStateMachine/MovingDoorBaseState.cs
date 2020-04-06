using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovingDoorStateMachine
{
    public abstract class MovingDoorBaseState : State
    {
        private MovingDoor _movingDoor;
        protected MovingDoor MovingDoor => _movingDoor = _movingDoor ? _movingDoor : (MovingDoor)owner;
        //protected GameObject triggerCollider { get => MovingDoor.GetTriggerCollider(); }
    }
    

}
