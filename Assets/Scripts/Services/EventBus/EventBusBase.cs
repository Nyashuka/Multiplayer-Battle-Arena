using System.Collections.Generic;
using Services.EventBus.EventBusArguments;

namespace Services.EventBus
{
    public class EventBusBase
    {
        private readonly Dictionary<string, List<EventBusHandler>> _handlers = new();
        private readonly Dictionary<string, IEventBusArgs> _stickyEvents = new(); 

        public void Subscribe(string eventName, EventBusHandler action, bool isSticky = false)
        {
            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<EventBusHandler>());
            }
            
            _handlers[eventName].Add(action);
            
            if (isSticky && _stickyEvents.TryGetValue(eventName, out var args))
            {
                action?.Invoke(args);
            }
        }
        
        public void Unsubscribe(string eventName, EventBusHandler action)
        {
            if (_handlers.TryGetValue(eventName, out var handlers))
            {
                handlers.Remove(action);
            }
        }

        public void RaiseEvent(string eventName, IEventBusArgs args, bool sticky = false )
        {
            if (sticky)
            {
                _stickyEvents[eventName] = args;
            }

            if (!_handlers.TryGetValue(eventName, out var subscribers)) return;

            foreach (var subscriber in subscribers)
            {
                subscriber(args);
            }
        }
    }
}