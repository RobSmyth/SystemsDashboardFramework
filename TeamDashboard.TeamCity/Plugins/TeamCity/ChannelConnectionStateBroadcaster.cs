using System;
using NoeticTools.TeamStatusBoard.Framework;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
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
            onConnectedBroadcaster.AddListener(OnConnectedHandler);
            onDisconnectedBroadcaster.AddListener(OnDisconnectedHandler);
            onConnectedBroadcaster.AddListenersAddedListener(OnConnectedListenerAdded);
            onDisconnectedBroadcaster.AddListenersAddedListener(OnDisconnectedListenerAdded);
        }

        public EventBroadcaster OnConnected { get; }
        public EventBroadcaster OnDisconnected { get; }

        private void OnDisconnectedListenerAdded(Action callback)
        {
            _onConnectedListenerAdded(callback);
        }

        private void OnConnectedListenerAdded(Action callback)
        {
            _onDisconnectedListenerAdded(callback);
        }

        private void OnDisconnectedHandler()
        {
            _onDisconnectedListenerAdded = x => {};
            _onConnectedListenerAdded = x => x();
        }

        private void OnConnectedHandler()
        {
            _onDisconnectedListenerAdded = x => {};
            _onConnectedListenerAdded = x => x();
        }
    }
}
