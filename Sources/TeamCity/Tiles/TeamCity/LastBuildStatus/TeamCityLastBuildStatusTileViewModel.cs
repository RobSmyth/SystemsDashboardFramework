using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using log4net;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Configurations;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.LastBuildStatus
{
    internal sealed class TeamCityLastBuildStatusTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel, IChannelConnectionStateListener
    {
        public const string TileTypeId = "TeamCity.Build.Status";
        private readonly IServices _services;
        private readonly TeamCityBuildStatusTileControl _view;
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();
        private string _status;
        private string _description;
        private static int _nextInstanceId = 1;
        private string _buildVersion;
        private int _agentsCount;
        private string _runningStatus;
        private readonly INamedValueRepository _configurationNamedValues;
        private readonly INamedValueRepository _namedValues;

        public TeamCityLastBuildStatusTileViewModel(ITeamCityChannel channel, TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, TeamCityBuildStatusTileControl view, ITileProperties properties)
        {
            lock (_syncRoot)
            {
                _logger = LogManager.GetLogger($"Tiles.TeamCity.LastBuildStatus.{_nextInstanceId++}");
            }

            _services = services;
            _configurationNamedValues = properties.Properties;
            _namedValues = properties.NamedValueRepository;
            _view = view;
            _status = "UNKNOWN";
            _runningStatus = "UNKNOWN";
            _description = "";
            _buildVersion = "";

            var teamCityService = _services.GetServicesOfType<ITeamCityService>().First(); // todo - hack to introduce concept of multiple TeamCity services

            ConfigureServiceCommand = new DataSourceConfigureCommand(channel);
            var configurationParameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "Last Build Status Tile Configuration", configurationParameters, dashboardController, layoutController, services);

            RegisterProjectAndConfiguration();

            var stateBroadcaster = teamCityService.StateBroadcaster;
            teamCityService.ConnectedTicker.AddListener(this, Update);
            stateBroadcaster.Add(this);

            _view.DataContext = this;
        }

        private void RegisterProjectAndConfiguration()
        {
            var configurationName = _configurationNamedValues.GetString("Configuration");
            _namedValues.GetString(configurationName);
        }

        public string Status
        {
            get { return _status; }
            private set
            {
                if (!_status.Equals(value, StringComparison.InvariantCulture))
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public string RunningStatus
        {
            get { return _runningStatus; }
            private set
            {
                if (!_runningStatus.Equals(value, StringComparison.InvariantCulture))
                {
                    _runningStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get { return _description; }
            private set
            {
                if (!_description.Equals(value, StringComparison.InvariantCulture))
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BuildVersion
        {
            get { return _buildVersion; }
            private set
            {
                if (!_buildVersion.Equals(value, StringComparison.InvariantCulture))
                {
                    _buildVersion = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AgentsCount
        {
            get { return _agentsCount; }
            private set
            {
                if (_agentsCount != value)
                {
                    _agentsCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(INamedValueRepository converter)
        {
            _logger.Debug("Configuration changed.");
            Update();
        }

        private void Update()
        {
            _logger.Debug("Timer elapsed. Update.");

            var buildConfiguration = _namedValues.Get<BuildConfiguration>("Configuration");

            Task.Factory.StartNew(() => GetBuilds(buildConfiguration)).ContinueWith(x => _view.Dispatcher.InvokeAsync(() => Update(x.Result)));
        }

        private Build[] GetBuilds(BuildConfiguration buildConfiguration)
        {
            if (buildConfiguration == null)
            {
                return new Build[0];
            }

            var build = buildConfiguration.GetRunningBuilds();
            if (build.Any())
            {
                return build;
            }

            _logger.Debug("No build running, getting last build.");
            var lastBuild = buildConfiguration.GetLastBuild();
            if (lastBuild == null)
            {
                return new Build[0];
            }

            return new[] {lastBuild};
        }

        private void Update(Build[] builds)
        {
            var build = builds.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(build?.Status))
            {
                SetUiToError();
                return;
            }

            Status = build.Status;

            var running = build.Status.StartsWith("RUNNING");

            var status = build.Status;
            Description = _configurationNamedValues.GetString("Description");
            BuildVersion = build.Number;
            RunningStatus = running ? "Running" : "";
            _view.agentsCount.Visibility = running ? Visibility.Visible : Visibility.Collapsed;
            AgentsCount = builds.Length;

            _logger.DebugFormat("Updated UI. Build is {0}.", status);
        }

        private void SetUiToError()
        {
            _logger.Warn("Update UI - unable to get build state.");
            Status = "UNKNOWN";
            RunningStatus = "UNKNOWN";
            Description = "ERROR";
            BuildVersion = "";
            _view.agentsCount.Visibility = Visibility.Collapsed;
        }

        private IPropertyViewModel[] GetConfigurationParameters()
        {
            var configurationParameters = new IPropertyViewModel[]
            {
                new TeamCityConfigurationPropertyViewModel("Configuration", _configurationNamedValues, _services),
                new TextPropertyViewModel("Description", _configurationNamedValues, _services),
                new HyperlinkPropertyViewModel("TeamCity service", ConfigureServiceCommand),
            };
            return configurationParameters;
        }

        private ICommand ConfigureServiceCommand { get; }

        void IChannelConnectionStateListener.OnConnected()
        {
        }

        void IChannelConnectionStateListener.OnDisconnected()
        {
            SetUiToError();
        }
    }
}