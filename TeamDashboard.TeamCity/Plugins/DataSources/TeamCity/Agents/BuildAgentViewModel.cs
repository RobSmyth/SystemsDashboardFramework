using System;
using System.Collections.Generic;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents
{
    public class TeamCityBuildAgentViewModel : NotifyingViewModelBase, IBuildAgent, ITimerListener, IChannelConnectionStateListener
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly ITimerService _timer;
        private readonly IDataSource _outerRepository;
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private ITimerToken _timerToken = new NullTimerToken();
        private BuildAgentStatus _status;
        private bool _isRunning;
        private string _statusText;
        private Action _onDisconnectAction = () => { };
        private Action _onConnectAction = () => { };
        private bool? _isOnline;
        private bool? _isAuthorised;

        public TeamCityBuildAgentViewModel(string name, ITimerService timer, IDataSource outerRepository, IChannelConnectionStateBroadcaster channelStateBroadcaster, ITcSharpTeamCityClient teamCityClient)
        {
            _timer = timer;
            _outerRepository = outerRepository;
            _teamCityClient = teamCityClient;
            Name = name;
            _statusText = string.Empty;
            Status = BuildAgentStatus.Offline;
            SetDisconnectedStateActions();
            channelStateBroadcaster.Add(this);
        }

        public string Name { get; }

        // todo - needs to check agent's online state from server
        public bool IsOnline
        {
            get { return _isOnline.HasValue && _isOnline.Value; }
            set
            {
                if (!_isOnline.HasValue || _isOnline != value)
                {
                    _isOnline = value;
                    OnPropertyChanged();
                    if (Status == BuildAgentStatus.NotAuthorised)
                    {
                        return;
                    }

                    if (!IsOnline)
                    {
                        Status = BuildAgentStatus.Offline;
                    }
                    else if (Status == BuildAgentStatus.Offline || Status == BuildAgentStatus.Unknown)
                    {
                        Status = BuildAgentStatus.Idle;
                    }
                }
            }
        }

        public bool IsAuthorised
        {
            get { return _isAuthorised.HasValue && _isAuthorised.Value; }
            set
            {
                if (!_isAuthorised.HasValue || _isAuthorised != value)
                {
                    _isAuthorised = value;
                    OnPropertyChanged();
                    if (!IsAuthorised)
                    {
                        Status = BuildAgentStatus.NotAuthorised;
                    }
                    else
                    {
                        Status = BuildAgentStatus.Idle;
                    }
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
                    if (IsRunning)
                    {
                        Status = BuildAgentStatus.Running;
                    }
                    else if (Status == BuildAgentStatus.Running)
                    {
                        Status = BuildAgentStatus.Idle;
                    }
                }
            }
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
                    UpdateStateText();
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

            UpdateBuildAgentParameters();
            _timerToken.Requeue(_updatePeriod);
        }

        private void UpdateBuildAgentParameters()
        {
            _outerRepository.Write($"Agent.{Name}.Status", Status);
        }

        private void UpdateStateText()
        {
            var lookup = new Dictionary<BuildAgentStatus, string>()
            {
                {BuildAgentStatus.Unknown, "Unknown" },
                {BuildAgentStatus.Idle, "Idle" },
                {BuildAgentStatus.Offline, "Off-line" },
                {BuildAgentStatus.Disabled, "Disabled" },
                {BuildAgentStatus.NotAuthorised, "NotAuthorised" },
                {BuildAgentStatus.Running, "Running" },
            };
            StatusText = lookup[Status];
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