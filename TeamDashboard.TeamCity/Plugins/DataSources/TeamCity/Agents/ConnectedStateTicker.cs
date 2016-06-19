using System;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents
{
    public sealed class ConnectedStateTicker : IChannelConnectionStateListener, ITimerListener, IConnectedStateTicker
    {
        private readonly TimeSpan _updateDelayOnConnection = TimeSpan.FromSeconds(1.0);
        private readonly IEventBroadcaster _broadcaster;
        private readonly ITimerService _timerService;
        private readonly TimeSpan _tickPeriod;
        private Action _onConnected;
        private Action _onDisconnected;
        private Action _requeue;
        private Action<Action> _onAdded;
        private ITimerToken _timerToken = new NullTimerToken();

        public ConnectedStateTicker(IEventBroadcaster broadcaster, ITimerService timerService, TimeSpan tickPeriod, IChannelConnectionStateBroadcaster connectionStateBroadcaster)
        {
            _broadcaster = broadcaster;
            _timerService = timerService;
            _tickPeriod = tickPeriod;
            _onConnected = DoNothing;
            _onDisconnected = DoNothing;
            _requeue = DoNothing;
            _onAdded = x => {};
            connectionStateBroadcaster.Add(this);
        }

        public void AddListener(Action tickCallback)
        {
            _broadcaster.AddListener(tickCallback);
            _onAdded(tickCallback);
        }

        public void RemoveListener(Action tickCallback)
        {
            _broadcaster.RemoveListener(tickCallback);
        }

        void IChannelConnectionStateListener.OnConnected()
        {
            var action = _onConnected;
            EnterConnectedState();
            action();
        }

        void IChannelConnectionStateListener.OnDisconnected()
        {
            var action = _onDisconnected;
            EnterDisconnectedState();
            action();
        }

        private void EnterConnectedState()
        {
            _onDisconnected = () =>
            {
                var token = _timerToken;
                _timerToken = new NullTimerToken();
                token.Cancel();
            };
            _onConnected = DoNothing;
            _onAdded = x => x();
            _requeue = ()=> _timerToken = _timerService.QueueCallback(_tickPeriod, this);
        }

        private void EnterDisconnectedState()
        {
            _requeue = DoNothing;
            _onDisconnected = DoNothing;
            _onConnected = () =>
            {
                _timerToken.Cancel();
                _timerToken = _timerService.QueueCallback(_updateDelayOnConnection, this);
            };
            _onAdded = x => { };
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            _broadcaster.Fire();
            _requeue();
        }

        private void DoNothing() { }
    }
}