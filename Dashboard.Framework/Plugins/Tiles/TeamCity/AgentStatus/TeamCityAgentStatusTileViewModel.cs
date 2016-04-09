using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using log4net;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.TeamCity.AgentStatus;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.AgentStatus
{
    internal sealed class TeamCityAgentStatusTileViewModel : NotifyingViewModelBase, ITimerListener, IConfigurationChangeListener, ITileViewModel
    {
        private readonly Dictionary<BuildAgentStatus, Brush> _statusBrushes = new Dictionary<BuildAgentStatus, Brush>
        {
            {BuildAgentStatus.Disabled, Brushes.Gray},
            {BuildAgentStatus.Running, Brushes.Yellow},
            {BuildAgentStatus.Idle, (SolidColorBrush) (new BrushConverter().ConvertFrom("#FF448032"))},
            {BuildAgentStatus.Unknown, Brushes.Gray},
            {BuildAgentStatus.Offline, Brushes.Gray}
        };

        private readonly Dictionary<BuildAgentStatus, Brush> _statusTextBrushes = new Dictionary<BuildAgentStatus, Brush>
        {
            {BuildAgentStatus.Disabled, Brushes.White},
            {BuildAgentStatus.Running, Brushes.DarkSlateGray},
            {BuildAgentStatus.Idle, Brushes.White},
            {BuildAgentStatus.Unknown, Brushes.White},
            {BuildAgentStatus.Offline, Brushes.White}
        };

        private readonly TeamCityService _service;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly TimeSpan _connectedUpdatePeriod = TimeSpan.FromSeconds(30);
        private readonly TimeSpan _disconnectedUpdatePeriod = TimeSpan.FromSeconds(2);
        private readonly TimerToken _timerToken;
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();
        private readonly TeamCityAgentStatusTileControl _view;
        private readonly ITeamCityChannel _teamCityService;
        private IBuildAgent _buildAgent;
        private static int _nextInstanceId = 1;

        public TeamCityAgentStatusTileViewModel(TeamCityService service, TileConfiguration tile, IDashboardController dashboardController, TileLayoutController tileLayoutController, IServices services, TeamCityAgentStatusTileControl view, ITeamCityChannel teamCityService)
        {
            lock (_syncRoot)
            {
                _logger = LogManager.GetLogger($"Tiles.TeamCity.Agent.{_nextInstanceId++}");
            }

            Tile = tile;
            _service = service;
            _view = view;
            _teamCityService = teamCityService;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            ConfigureServiceCommand = new TeamCityServiceConfigureCommand(service);

            var configurationParameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(Tile, "Build Agent Configuration", configurationParameters, dashboardController, tileLayoutController, services);
            GetBuildAgent();

            _view.DataContext = this;

            _timerToken = services.Timer.QueueCallback(TimeSpan.FromSeconds(_service.IsConnected ? 1 : 3), this);
        }

        private void GetBuildAgent()
        {
            BuildAgent = _teamCityService.GetAgent(_tileConfigurationConverter.GetString("AgentName")).Result;
        }

        public IBuildAgent BuildAgent
        {
            get { return _buildAgent; }
            private set
            {
                if (!ReferenceEquals(_buildAgent, value))
                {
                    _buildAgent = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            _logger.Debug("Configuration changed.");
            GetBuildAgent();
            _timerToken.Requeue(TimeSpan.FromMilliseconds(300));
        }

        public void OnTimeElapsed(TimerToken token)
        {
            if (!_service.IsConnected)
            {
                SetUiToError();
                _timerToken.Requeue(_disconnectedUpdatePeriod);
                return;
            }

            _logger.Debug("Timer elapsed. Update.");

            Task.Factory.StartNew(() => _service.GetAgents().Result).ContinueWith(x => _view.Dispatcher.InvokeAsync(() => Update(x.Result)));
        }

        private void Update(IBuildAgent[] agents)
        {
            var agentName = _tileConfigurationConverter.GetString("AgentName");

            var agent = agents.FirstOrDefault(x => x.Name.Equals(agentName, StringComparison.InvariantCulture));
            if (agent == null)
            {
                SetUiToError();
                _timerToken.Requeue(_disconnectedUpdatePeriod);
                return;
            }

            _view.root.Background = _statusBrushes[BuildAgent.Status];

            _logger.DebugFormat("Updated UI. Build is {0}.", BuildAgent.Status);

            _timerToken.Requeue(_connectedUpdatePeriod);
        }

        private void SetUiToError()
        {
            _logger.Warn("Update UI - unable to get build state.");
            _view.root.Background = _statusBrushes[BuildAgentStatus.Unknown];
        }

        private IPropertyViewModel[] GetConfigurationParameters()
        {
            var configurationParameters = new IPropertyViewModel[]
            {
                new PropertyViewModel("AgentName", "Text", _tileConfigurationConverter)
            };
            return configurationParameters;
        }

        private TileConfiguration Tile { get; }

        private ICommand ConfigureServiceCommand { get; }
    }
}