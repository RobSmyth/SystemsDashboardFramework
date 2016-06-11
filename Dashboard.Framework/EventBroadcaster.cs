using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoeticTools.TeamStatusBoard.Framework
{
    public class EventBroadcaster : IEventBroadcaster
    {
        private readonly IList<Action> _callbacks = new List<Action>();
        private readonly IList<Action<Action>> _listenerAddedCallbacks = new List<Action<Action>>();

        public void AddListener(Action callback)
        {
            _callbacks.Add(callback);
            NotifyNewListenerAdded(callback);
        }

        public void RemoveListener(Action callback)
        {
            _callbacks.Remove(callback);
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
                listener();
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
