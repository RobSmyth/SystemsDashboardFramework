using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using log4net;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.LastBuildStatus;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.Tiles.TeamCity.LastBuildStatus
{
    internal sealed class TeamCityLastBuildStatusTileViewModel : NotifyingViewModelBase, ITimerListener, IConfigurationChangeListener, ITileViewModel
    {
        public const string TileTypeId = "TeamCity.Build.Status";
        private readonly ITeamCityChannel _channel;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly TimeSpan _connectedUpdatePeriod = TimeSpan.FromSeconds(30);
        private readonly TimeSpan _disconnectedUpdatePeriod = TimeSpan.FromSeconds(5);
        private readonly ITimerToken _timerToken;
        private readonly TeamCityBuildStatusTileControl _view;
        private readonly IProjectRepository _projectRepository;
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();
        private string _status;
        private string _description;
        private static int _nextInstanceId = 1;
        private string _buildVersion;
        private int _agentsCount;
        private string _runningStatus;

        public TeamCityLastBuildStatusTileViewModel(ITeamCityChannel channel, TileConfiguration tile, IDashboardController dashboardController, 
            ITileLayoutController layoutController, IServices services, TeamCityBuildStatusTileControl view, IProjectRepository projectRepository)
        {
            lock (_syncRoot)
            {
                _logger = LogManager.GetLogger($"Tiles.TeamCity.LastBuildStatus.{_nextInstanceId++}");
            }

            _channel = channel;
            _view = view;
            _projectRepository = projectRepository;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            _status = "UNKNOWN";
            _runningStatus = "UNKNOWN";
            _description = "";
            _buildVersion = "";
            ConfigureServiceCommand = new DataSourceConfigureCommand(channel);
            var configurationParameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "Last Build Status Tile Configuration", configurationParameters, dashboardController, layoutController, services);
            _view.DataContext = this;
            _timerToken = services.Timer.QueueCallback(TimeSpan.FromSeconds(3.0), this);
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

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            _logger.Debug("Configuration changed.");
            _timerToken.Requeue(TimeSpan.FromMilliseconds(300));
        }

        public void OnTimeElapsed(TimerToken token)
        {
            if (!_channel.IsConnected)
            {
                SetUiToError();
                _timerToken.Requeue(_disconnectedUpdatePeriod);
                return;
            }

            _logger.Debug("Timer elapsed. Update.");

            var projectName = _tileConfigurationConverter.GetString("Project");
            var configurationName = _tileConfigurationConverter.GetString("Configuration");

            Task.Factory.StartNew(() => GetBuilds(projectName, configurationName)).ContinueWith(x => _view.Dispatcher.InvokeAsync(() => Update(x.Result)));
        }

        private Build[] GetBuilds(string projectName, string configurationName)
        {
            var build = _projectRepository.Get(projectName).GetConfiguration(configurationName).GetRunningBuilds();
            if (build.Any())
            {
                return build;
            }

            _logger.Debug("No build running, getting last build.");
            var lastBuild = _channel.Projects.Get(projectName).GetConfiguration(configurationName).GetLastBuild();
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
                _timerToken.Requeue(_disconnectedUpdatePeriod);
                return;
            }

            Status = build.Status;

            var running = build.Status.StartsWith("RUNNING");

            var status = build.Status;
            Description = _tileConfigurationConverter.GetString("Description");
            BuildVersion = build.Number;
            RunningStatus = running ? "Running" : "";
            _view.agentsCount.Visibility = running ? Visibility.Visible : Visibility.Collapsed;
            AgentsCount = builds.Length;

            _logger.DebugFormat("Updated UI. Build is {0}.", status);

            _timerToken.Requeue(_connectedUpdatePeriod);
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
            var projectElementViewModel = new TeamCityProjectPropertyViewModel("Project", _tileConfigurationConverter, _channel);
            var configurationParameters = new IPropertyViewModel[]
            {
                
                projectElementViewModel,
                new DependantPropertyViewModel("Configuration", "TextFromCombobox", _tileConfigurationConverter, projectElementViewModel,
                    () => _channel.Projects.Get((string) projectElementViewModel.Value).Configurations.Select(x => x.Name).Cast<object>().ToArray()),
                new PropertyViewModel("Description", "Text", _tileConfigurationConverter),
                new HyperlinkPropertyViewModel("TeamCity service", ConfigureServiceCommand)
            };
            return configurationParameters;
        }

        private ICommand ConfigureServiceCommand { get; }
    }
}