using System;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public class TeamCityBuildAgentViewModel : NotifyingViewModelBase, IBuildAgent, ITimerListener, IChannelConnectionStateListener
    {
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);
        private readonly ITimerService _timer;
        private readonly IDataSource _outerRepository;
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private ITimerToken _timerToken = new NullTimerToken();
        private BuildAgentStatus _status;
        private bool _isRunning;
        private string _statusText;
        private Action _onDisconnectAction = () => { };
        private Action _onConnectAction = () => { };
        private bool _isOnline;

        public TeamCityBuildAgentViewModel(string name, ITimerService timer, IDataSource outerRepository, IChannelConnectionStateBroadcaster channelStateBroadcaster, ITcSharpTeamCityClient teamCityClient)
        {
            _timer = timer;
            _outerRepository = outerRepository;
            _teamCityClient = teamCityClient;
            Name = name;
            _statusText = string.Empty;
            SetDisconnectedStateActions();
            channelStateBroadcaster.Add(this);
        }

        public string Name { get; }

        // todo - needs to check agent's online state from server
        public bool IsOnline
        {
            get { return _isOnline; }
            set
            {
                if (_isOnline != value)
                {
                    _isOnline = value;
                    OnPropertyChanged();
                }
            }
        }

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
            IsOnline = false;
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
            var runningProjects = _teamCityClient.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: "default:any", agentName:Name)).ToArray();
            IsRunning = runningProjects.Length == 1;
            Status = IsRunning ? BuildAgentStatus.Running : BuildAgentStatus.Idle;
            StatusText = IsRunning ? "Running" : "Idle";

            UpdateBuildAgentParameters();
            _timerToken.Requeue(_tickPeriod);
        }

        private void UpdateBuildAgentParameters()
        {
            _outerRepository.Write($"Agent.{Name}.Status", Status);
        }

        void IChannelConnectionStateListener.OnConnected()
        {
            var action = _onConnectAction;
            SetConnectedStateActions();
            action();
        }

        void IChannelConnectionStateListener.OnDisconnected()
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
                Status = BuildAgentStatus.Unknown;
                StatusText = "Unknown";
                IsOnline = false;
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