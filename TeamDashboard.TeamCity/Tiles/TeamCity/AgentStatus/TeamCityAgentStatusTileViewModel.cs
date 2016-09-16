using System.Windows.Input;
using log4net;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.AgentStatus
{
    internal sealed class TeamCityAgentStatusTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();
        private readonly ITeamCityService _teamCityService;
        private IBuildAgent _buildAgent = new NullBuildAgent("");
        private static int _nextInstanceId = 1;

        public TeamCityAgentStatusTileViewModel(ITeamCityChannel channel, TileConfiguration tile, 
            IDashboardController dashboardController, ITileLayoutController tileLayoutController, IServices services, 
            TeamCityAgentStatusTileControl view, ITeamCityService teamCityService)
        {
            lock (_syncRoot)
            {
                _logger = LogManager.GetLogger($"Tiles.TeamCity.Agent.{_nextInstanceId++}");
            }

            Tile = tile;
            _teamCityService = teamCityService;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            ConfigureServiceCommand = new DataSourceConfigureCommand(channel);

            var configurationParameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(Tile, "Build Agent Configuration", configurationParameters, dashboardController, tileLayoutController, services);
            GetBuildAgent();

            view.DataContext = this;
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
        }

        private void GetBuildAgent()
        {
            BuildAgent = _teamCityService.Agents.Get(_tileConfigurationConverter.GetString("AgentName"));
        }

        private IPropertyViewModel[] GetConfigurationParameters()
        {
            var configurationParameters = new IPropertyViewModel[]
            {
                new PropertyViewModel("AgentName", PropertyType.Text, _tileConfigurationConverter)
            };
            return configurationParameters;
        }

        private TileConfiguration Tile { get; }

        private ICommand ConfigureServiceCommand { get; }
    }
}