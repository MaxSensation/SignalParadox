using UnityEngine;

namespace EventSystem
{
    public abstract class EventInfo
    {
        public string eventDescription;
    }

    public class DebugEventInfo : EventInfo
    {
        public int verbosityLevel;
    }
    
    // Pickup Events
    public class OnPickupStunBatonEvent : EventInfo {}
    public class OnPickupStunGunUpgradeEvent : EventInfo {}
    
    // Trigger Events
    public class OnTriggerMemoEvent : EventInfo
    {
        public readonly AudioClip memoAudioClip;
        public OnTriggerMemoEvent(AudioClip memoAudioClip)
        {
            this.memoAudioClip = memoAudioClip;
        }
    }
  
    public class OnTriggerEnteredCheckPointEvent : EventInfo {}
    public class OnCheckPointLoadEvent : EventInfo {}

    public class OnCheckPointLoadedEvent : EventInfo
    {
        public readonly CheckPoint checkPoint;
        public OnCheckPointLoadedEvent(CheckPoint checkPoint)
        {
            this.checkPoint = checkPoint;
        }
    }
    public class OnCheckPointSaveEvent : EventInfo
    {
        public readonly CheckPoint checkPoint;
        public OnCheckPointSaveEvent(CheckPoint checkPoint)
        {
            this.checkPoint = checkPoint;
        }
    }

    // Level Events
    public class OnLevelFirstMemoTriggeredEvent : EventInfo {}
    
    // Menu Events
    public class OnButtonStartEvent : EventInfo
    {
        public readonly string level;
        public OnButtonStartEvent(string level)
        {
            this.level = level;
        }
    }
    
    // Player Events
    public class OnPlayerEnteredInteractionEvent : EventInfo
    {
        public readonly GameObject interactableObject ;
        public OnPlayerEnteredInteractionEvent(GameObject gameObject)
        {
            interactableObject = gameObject;
        }
    }
    
    public class OnPlayerExitedInteractionEvent : EventInfo
    {
        public readonly GameObject interactableObject ;
        public OnPlayerExitedInteractionEvent(GameObject gameObject)
        {
            interactableObject = gameObject;
        }
    }
    
    public class OnPlayerDieEvent : EventInfo{}
    
    // Button Events
    public class OnButtonPressedEvent : EventInfo
    {
        public readonly GameObject[] interactableObjects ;
        public OnButtonPressedEvent(GameObject[] gameObjects)
        {
            interactableObjects = gameObjects;
        }
    }
}
