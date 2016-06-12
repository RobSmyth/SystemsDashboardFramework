using System;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public class TeamCityBuildAgentViewModel : NotifyingViewModelBase, IBuildAgent, ITimerListener
    {
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);
        private readonly ITimerService _timer;
        private readonly IDataSource _outerRepository;
        private ITimerToken _timerToken = new NullTimerToken();
        private BuildAgentStatus _status;
        private bool _isRunning;
        private string _statusText;
        private Action _onDisconnectAction = () => { };
        private Action _onConnectAction = () => { };

        public TeamCityBuildAgentViewModel(string name, ITimerService timer, IDataSource outerRepository, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _timer = timer;
            _outerRepository = outerRepository;
            Name = name;
            _statusText = string.Empty;
            SetDisconnectedStateActions();
            channelStateBroadcaster.OnDisconnected.AddListener(OnDisconnected);
            channelStateBroadcaster.OnConnected.AddListener(OnConnected);
        }

        public string Name { get; }

        public bool IsRunning
        {
            get { return _isRunning; }
            private set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    OnPropertyChanged();
                }
            }
        }

        public void IsNotKnown()
        {
            Status = BuildAgentStatus.Unknown;
            StatusText = "Unknown";
            IsRunning = false;
        }

        public BuildAgentStatus Status
        {
            get { return _status; }
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            private set
            {
                if (!_statusText.Equals(value, StringComparison.InvariantCulture))
                {
                    _statusText = value;
                    OnPropertyChanged();
                }
            }
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
        }

        private void Update()
        {
            UpdateBuildAgentParameters();
            _timerToken.Requeue(_tickPeriod);
        }

        private void UpdateBuildAgentParameters()
        {
            _outerRepository.Write($"Agent.{Name}.Status", Status);
        }

        private void OnConnected()
        {
            var action = _onConnectAction;
            SetConnectedStateActions();
            action();
        }

        private void OnDisconnected()
        {
            var action = _onDisconnectAction;
            SetDisconnectedStateActions();
            action();
        }

        private void SetConnectedStateActions()
        {
            _onConnectAction = () => { };
            _onDisconnectAction = () =>
            {
                var token = _timerToken;
                _timerToken = new NullTimerToken();
                token.Cancel();
            };
        }

        private void SetDisconnectedStateActions()
        {
            _onConnectAction = () =>
            {
                _timerToken.Cancel();
                _onDisconnectAction = () => { };
                _timerToken = _timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);
            };
        }
    }
}