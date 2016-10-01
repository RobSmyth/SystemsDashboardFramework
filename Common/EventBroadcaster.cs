using System;
using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.TeamStatusBoard.Common
{
    public class EventBroadcaster : IEventBroadcaster
    {
        private readonly IDictionary<object, Action> _callbacks = new Dictionary<object, Action>();
        private readonly IList<Action<Action>> _listenerAddedCallbacks = new List<Action<Action>>();

        public void AddListener(object listener, Action callback)
        {
            _callbacks.Add(listener, callback);
            NotifyNewListenerAdded(callback);
        }

        public void RemoveListener(object listener)
        {
            _callbacks.Remove(listener);
        }

        public void Flush()
        {
            _callbacks.Clear();
        }

        public void Fire()
        {
            var listeners = _callbacks.ToArray();
            foreach (var listener in listeners)
            {
                listener.Value();
            }
        }

        public void AddListenersAddedListener(Action<Action> callback)
        {
            _listenerAddedCallbacks.Add(callback);
        }

        private void NotifyNewListenerAdded(Action callback)
        {
            foreach (var listener in _listenerAddedCallbacks)
            {
                listener(callback);
            }
        }
    }
}
