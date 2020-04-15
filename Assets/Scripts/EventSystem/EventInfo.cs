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
    
    // Level Events
    public class OnLevelFirstMemoTriggeredEvent : EventInfo {}
}
