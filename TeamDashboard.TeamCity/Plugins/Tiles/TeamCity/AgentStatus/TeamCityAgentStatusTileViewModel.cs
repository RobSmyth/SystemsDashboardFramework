using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using log4net;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.AgentStatus;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.Tiles.TeamCity.AgentStatus
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

        private readonly ITeamCityChannel _channel;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly TimeSpan _connectedUpdatePeriod = TimeSpan.FromSeconds(30);
        private readonly TimeSpan _disconnectedUpdatePeriod = TimeSpan.FromSeconds(2);
        private readonly ITimerToken _timerToken;
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();
        private readonly TeamCityAgentStatusTileControl _view;
        private readonly ITeamCityChannel _teamCityChannel;
        private IBuildAgent _buildAgent;
        private static int _nextInstanceId = 1;

        public TeamCityAgentStatusTileViewModel(ITeamCityChannel channel, TileConfiguration tile, IDashboardController dashboardController, 
            ITileLayoutController tileLayoutController, IServices services,
            TeamCityAgentStatusTileControl view, ITeamCityChannel teamCityChannel)
        {
            lock (_syncRoot)
            {
                _logger = LogManager.GetLogger($"Tiles.TeamCity.Agent.{_nextInstanceId++}");
            }

            Tile = tile;
            _channel = channel;
            _view = view;
            _teamCityChannel = teamCityChannel;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            ConfigureServiceCommand = new DataSourceConfigureCommand(channel);

            var configurationParameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(Tile, "Build Agent Configuration", configurationParameters, dashboardController, tileLayoutController, services);
            GetBuildAgent();

            _view.DataContext = this;

            _timerToken = services.Timer.QueueCallback(TimeSpan.FromSeconds(_channel.IsConnected ? 1 : 3), this);
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
            if (!_channel.IsConnected)
            {
                SetUiToError();
                _timerToken.Requeue(_disconnectedUpdatePeriod);
                return;
            }

            _logger.Debug("Timer elapsed. Update.");

            Update();
        }

        private void GetBuildAgent()
        {
            BuildAgent = _teamCityChannel.GetAgent(_tileConfigurationConverter.GetString("AgentName")).Result;
        }

        private void Update()
        {
            var agentName = _tileConfigurationConverter.GetString("AgentName");

            var agent = _channel.Agents.Get(agentName);
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