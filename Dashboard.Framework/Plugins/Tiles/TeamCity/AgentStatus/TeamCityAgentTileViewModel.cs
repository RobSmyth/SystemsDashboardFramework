using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using log4net;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity;
using NoeticTools.SystemsDashboard.Framework.Time;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.TeamCity.AgentStatus
{
    internal sealed class TeamCityAgentTileViewModel : NotifyingViewModelBase, ITimerListener, IConfigurationChangeListener, ITileViewModel
    {
        private readonly Dictionary<string, Brush> _statusBrushes = new Dictionary<string, Brush>
        {
            {"DISABLED", Brushes.Gray},
            {"RUNNING", Brushes.Yellow},
            {"IDLE", (SolidColorBrush) (new BrushConverter().ConvertFrom("#FF448032"))},
            {"FAILURE", Brushes.Firebrick},
            {"UNKNOWN", Brushes.Gray}
        };

        private readonly Dictionary<string, Brush> _statusTextBrushes = new Dictionary<string, Brush>
        {
            {"DISABLED", Brushes.White},
            {"RUNNING", Brushes.DarkSlateGray},
            {"IDLE", Brushes.White},
            {"FAILURE", Brushes.White},
            {"UNKNOWN", Brushes.White}
        };

        private readonly TeamCityService _service;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly TimeSpan _connectedUpdatePeriod = TimeSpan.FromSeconds(30);
        private readonly TimeSpan _disconnectedUpdatePeriod = TimeSpan.FromSeconds(2);
        private readonly TimerToken _timerToken;
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();
        private readonly TeamCityAgentStatusTileControl _view;
        private readonly IBuildAgentRepository _buildAgentRepository;
        private IBuildAgent _buildAgent;
        private static int _nextInstanceId = 1;

        public TeamCityAgentTileViewModel(TeamCityService service, TileConfiguration tile, IDashboardController dashboardController, TileLayoutController tileLayoutController, IServices services, TeamCityAgentStatusTileControl view, IBuildAgentRepository buildAgentRepository)
        {
            lock (_syncRoot)
            {
                _logger = LogManager.GetLogger($"Tiles.TeamCity.Agent.{_nextInstanceId++}");
            }

            Tile = tile;
            _service = service;
            _view = view;
            _buildAgentRepository = buildAgentRepository;
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
            BuildAgent = _buildAgentRepository.Get(_tileConfigurationConverter.GetString("AgentName"));
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
            _view.agentName.Text = agentName;

            var agent = agents.FirstOrDefault(x => x.Name.Equals(agentName, StringComparison.InvariantCulture));
            if (agent == null)
            {
                SetUiToError();
                _timerToken.Requeue(_disconnectedUpdatePeriod);
                return;
            }

            var running = false; //agent.Status.StartsWith("RUNNING");

            var status = "IDLE";
            _view.headerText.Text = running ? "Running" : "Idle";
            _view.root.Background = _statusBrushes[status];
            SetTextForeground(running, status);

            _logger.DebugFormat("Updated UI. Build is {0}.", status);

            _timerToken.Requeue(_connectedUpdatePeriod);
        }

        private void SetUiToError()
        {
            _logger.Warn("Update UI - unable to get build state.");
            _view.agentName.Text = "ERROR";
            _view.headerText.Text = "";
            _view.root.Background = _statusBrushes["UNKNOWN"];
        }

        private IPropertyViewModel[] GetConfigurationParameters()
        {
            var configurationParameters = new IPropertyViewModel[]
            {
                new PropertyViewModel("AgentName", "Text", _tileConfigurationConverter)
            };
            return configurationParameters;
        }

        private void SetTextForeground(bool running, string status)
        {
            var textBrush = running ? _statusTextBrushes[status] : _statusTextBrushes[status];
            _view.headerText.Foreground = textBrush;
        }

        private TileConfiguration Tile { get; }

        private ICommand ConfigureServiceCommand { get; }
    }
}