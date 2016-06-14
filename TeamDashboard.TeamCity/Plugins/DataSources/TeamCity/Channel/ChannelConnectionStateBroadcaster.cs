using System;
using NoeticTools.TeamStatusBoard.Framework;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel
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
            OnConnected.AddListener(EnterConnectedState);
            OnDisconnected.AddListener(EnterDisconnectedState);
            OnConnected.AddListenersAddedListener(OnConnectedListenerAdded);
            OnDisconnected.AddListenersAddedListener(OnDisconnectedListenerAdded);
        }

        public EventBroadcaster OnConnected { get; }
        public EventBroadcaster OnDisconnected { get; }

        public void Add(IChannelConnectionStateListener listener)
        {
            OnConnected.AddListener(listener.OnConnected);
            OnDisconnected.AddListener(listener.OnDisconnected);
        }

        public void Remove(IChannelConnectionStateListener listener)
        {
            OnConnected.RemoveListener(listener.OnConnected);
            OnDisconnected.RemoveListener(listener.OnDisconnected);
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
