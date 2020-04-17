using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventSystem
{
    public static class EventHandler
    {
        private delegate void EventListener(EventInfo e);
        private static Dictionary<Type, List<EventListener>> _eventListeners;
        
        public static void RegisterListener<T>(Action<T> listener) where T : EventInfo
        {
            var eventType = typeof(T);
            if (_eventListeners == null)
                _eventListeners = new Dictionary<Type, List<EventListener>>();
            if (_eventListeners.ContainsKey(eventType) == false || _eventListeners[eventType] == null)
                _eventListeners[eventType] = new List<EventListener>();
            EventListener wrapper = (ei) => {listener((T)ei);};
            _eventListeners[eventType].Add(wrapper);
        }
        
        public static void UnregisterListener<T>(Action<T> listener) where T : EventInfo
        {
            var eventType = typeof(T);
            if (_eventListeners == null || _eventListeners.ContainsKey(eventType) == false || _eventListeners[eventType] == null)
                return;
            EventListener wrapper = (ei) => {listener((T)ei);};
            _eventListeners[eventType].Remove(wrapper);
        }

        public static void InvokeEvent(EventInfo eventInfo)
        {
            var correctEventInfoClass = eventInfo.GetType();
            if (_eventListeners?[correctEventInfoClass] == null)
                return;
            for (var i = 0; i < _eventListeners[correctEventInfoClass].Count; i++)
                _eventListeners[correctEventInfoClass][i](eventInfo);
        }
    }
}
