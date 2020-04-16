﻿using System.Collections.Generic;

namespace EventSystem
{
    public static class EventHandler
    {
        private delegate void EventListener(EventInfo e);
        private static Dictionary<System.Type, List<EventListener>> _eventListeners;
        
        public static void RegisterListener<T>(System.Action<T> listener) where T : EventInfo
        {
            var eventType = typeof(T);
            if (_eventListeners == null)
                _eventListeners = new Dictionary<System.Type, List<EventListener>>();
            if (_eventListeners.ContainsKey(eventType) == false || _eventListeners[eventType] == null)
                _eventListeners[eventType] = new List<EventListener>();
            EventListener wrapper = (ei) => {listener((T)ei);};
            _eventListeners[eventType].Add(wrapper);
        }
        
        public static void UnregisterListener<T>(System.Action<T> listener) where T : EventInfo
        {
            var eventType = typeof(T);
            if (_eventListeners == null)
                return;
            if (_eventListeners.ContainsKey(eventType) == false || _eventListeners[eventType] == null)
                return;
            EventListener wrapper = (ei) => {listener((T)ei);};
            _eventListeners[eventType].Remove(wrapper);
        }

        public static void InvokeEvent(EventInfo eventInfo)
        {
            System.Type correctEventInfoClass = eventInfo.GetType();
            if (_eventListeners?[correctEventInfoClass] == null)
                return;
            for (var i = 0; i < _eventListeners[correctEventInfoClass].Count; i++)
                _eventListeners[correctEventInfoClass][i](eventInfo);
        }
    }
}