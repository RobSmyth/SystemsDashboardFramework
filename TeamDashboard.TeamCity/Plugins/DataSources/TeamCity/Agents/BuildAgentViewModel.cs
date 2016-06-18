using System;
using System.Collections.Generic;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents
{
    public class BuildAgentViewModel : NotifyingViewModelBase, IBuildAgent, IChannelConnectionStateListener
    {
        private readonly IDictionary<BuildAgentStatus, string> _statusTextLookup = new Dictionary<BuildAgentStatus, string>()
            {
                {BuildAgentStatus.Unknown, "Unknown" },
                {BuildAgentStatus.Idle, "Idle" },
                {BuildAgentStatus.Offline, "Off-line" },
                {BuildAgentStatus.Disabled, "Disabled" },
                {BuildAgentStatus.NotAuthorised, "Not authorised" },
                {BuildAgentStatus.Running, "Running" },
            };

        private readonly IDataSource _outerRepository;
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private BuildAgentStatus _status;
        private bool _isRunning;
        private string _statusText;
        private Action _onDisconnectAction;
        private Action _onConnectAction;
        private bool? _isOnline;
        private bool? _isAuthorised;

        public BuildAgentViewModel(string name, IDataSource outerRepository, IChannelConnectionStateBroadcaster channelStateBroadcaster, 
            ITcSharpTeamCityClient teamCityClient, IConnectedStateTicker connectedStateTicker)
        {
            _outerRepository = outerRepository;
            _teamCityClient = teamCityClient;
            Name = name;
            _statusText = string.Empty;
            Status = BuildAgentStatus.Offline;
            SetDisconnectedStateActions();
            connectedStateTicker.AddListener(OnTick);
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
                    StatusText = _statusTextLookup[Status];
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

        private void OnTick()
        {
            var runningProjects = _teamCityClient.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: "default:any", agentName:Name)).ToArray();
            IsRunning = runningProjects.Length == 1;

            UpdateBuildAgentParameters();
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
            _onConnectAction = DoNothing;
            _onDisconnectAction = () =>
            {
                Status = BuildAgentStatus.Unknown;
                StatusText = "Unknown";
                IsOnline = false;
            };
        }

        private void SetDisconnectedStateActions()
        {
            _onConnectAction = () =>
            {
                _onDisconnectAction = DoNothing;
            };
            _onDisconnectAction = DoNothing;
        }

        private void DoNothing() { }
    }
}