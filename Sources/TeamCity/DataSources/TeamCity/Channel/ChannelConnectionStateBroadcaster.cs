using System;
using NoeticTools.TeamStatusBoard.Common;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel
{
    public class ChannelConnectionStateBroadcaster : IChannelConnectionStateBroadcaster
    {
        private Action<Action> _onConnectedListenerAdded;
        private Action<Action> _onDisconnectedListenerAdded;

        public ChannelConnectionStateBroadcaster(EventBroadcaster onConnectedBroadcaster, EventBroadcaster onDisconnectedBroadcaster)
        {
            OnConnected = onConnectedBroadcaster;
            OnDisconnected = onDisconnectedBroadcaster;
            _onConnectedListenerAdded = x => { };
            _onDisconnectedListenerAdded = x => { };
            OnConnected.AddListener(this, EnterConnectedState);
            OnDisconnected.AddListener(this, EnterDisconnectedState);
            OnConnected.AddListenersAddedListener(OnConnectedListenerAdded);
            OnDisconnected.AddListenersAddedListener(OnDisconnectedListenerAdded);
            EnterDisconnectedState();
        }

        public EventBroadcaster OnConnected { get; }
        public EventBroadcaster OnDisconnected { get; }

        public void Add(IChannelConnectionStateListener listener)
        {
            OnConnected.AddListener(listener, listener.OnConnected);
            OnDisconnected.AddListener(listener, listener.OnDisconnected);
        }

        public void Remove(IChannelConnectionStateListener listener)
        {
            OnConnected.RemoveListener(listener);
            OnDisconnected.RemoveListener(listener);
        }

        private void OnDisconnectedListenerAdded(Action callback)
        {
            _onDisconnectedListenerAdded(callback);
        }

        private void OnConnectedListenerAdded(Action callback)
        {
            _onConnectedListenerAdded(callback);
        }

        private void EnterDisconnectedState()
        {
            _onDisconnectedListenerAdded = x => x();
            _onConnectedListenerAdded = x => {};
        }

        private void EnterConnectedState()
        {
            _onDisconnectedListenerAdded = x => {};
            _onConnectedListenerAdded = x => x();
        }
    }
}
