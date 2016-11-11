using System;
using System.Collections.Generic;
using NoeticTools.TeamStatusBoard.Common;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TcSharpInterop;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Agents
{
    public class BuildAgentViewModel : NotifyingViewModelBase, IBuildAgent, IChannelConnectionStateListener
    {
        private const string PropertyTag = "TeamCity.BuildAgent";

        private readonly IDictionary<DeviceStatus, string> _statusTextLookup = new Dictionary<DeviceStatus, string>()
            {
                {DeviceStatus.Unknown, "Unknown" },
                {DeviceStatus.Idle, "Idle" },
                {DeviceStatus.Offline, "Off-line" },
                {DeviceStatus.Disabled, "Disabled" },
                {DeviceStatus.NotAuthorised, "Not authorised" },
                {DeviceStatus.Running, "Running" },
            };

        private readonly IDataSource _dataSource;
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private DeviceStatus _status;
        private bool _isRunning;
        private string _statusText;
        private Action _onDisconnectAction;
        private Action _onConnectAction;
        private bool? _isOnline;
        private bool? _isAuthorised;

        public BuildAgentViewModel(string name, IDataSource dataSource, IChannelConnectionStateBroadcaster channelStateBroadcaster, 
            ITcSharpTeamCityClient teamCityClient, IConnectedStateTicker connectedStateTicker)
        {
            _dataSource = dataSource;
            _teamCityClient = teamCityClient;
            Name = name;
            _statusText = string.Empty;
            Status = DeviceStatus.Unknown;
            SetDisconnectedStateActions();
            UpdateProperties();
            connectedStateTicker.AddListener(this, OnTick);
            channelStateBroadcaster.Add(this);
        }

        public string Name { get; }

        public bool IsOnline
        {
            get { return _isOnline.HasValue && _isOnline.Value; }
            set
            {
                if (!_isOnline.HasValue || _isOnline != value)
                {
                    _isOnline = value;
                    OnPropertyChanged();
                    UpdateStatus();
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
                    UpdateStatus();
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
                    UpdateStatus();
                }
            }
        }

        private void UpdateStatus()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                Status = DeviceStatus.Unknown;
                return;
            }

            if (!IsAuthorised)
            {
                Status = DeviceStatus.NotAuthorised;
                return;
            }

            if (!IsOnline)
            {
                Status = DeviceStatus.Offline;
            }
            else if (Status == DeviceStatus.Offline || Status == DeviceStatus.Unknown)
            {
                Status = DeviceStatus.Idle;
            }

            if (Status != DeviceStatus.Offline)
            {
                if (IsRunning)
                {
                    Status = DeviceStatus.Running;
                }
                else if (Status == DeviceStatus.Running)
                {
                    Status = DeviceStatus.Idle;
                }
            }
        }

        public DeviceStatus Status
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
            try
            {
                var runningProjects = _teamCityClient.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: "default:any", agentName: Name)).ToArray();
                IsRunning = runningProjects.Length == 1;
                UpdateProperties();
            }
            catch (Exception)
            {
            }
        }

        public void UpdateProperties()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return;
            }
            SetProperty("Status", Status);
            SetProperty("Name", Name, "Ref");
            SetProperty("IsAuthorised", IsAuthorised);
            SetProperty("IsOnline", IsOnline);
            SetProperty("IsRunning", IsRunning);
        }

        private void SetProperty<T>(string propertyName, T value, params string[] tags)
        {
            var tagList = new List<string>(new [] { PropertyTag});
            tagList.AddRange(tags);
            _dataSource.Set($"Agents.{Name}.{propertyName}", value, PropertiesFlags.ReadOnly, tagList.ToArray());
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
                Status = DeviceStatus.Unknown;
                StatusText = "Unknown";
                IsOnline = false;
                UpdateProperties();
            };
        }

        private void SetDisconnectedStateActions()
        {
            _onConnectAction = () =>
            {
                _onDisconnectAction = DoNothing;
                UpdateProperties();
            };
            _onDisconnectAction = DoNothing;
        }

        private void DoNothing() { }
    }
}