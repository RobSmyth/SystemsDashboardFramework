using System.Collections.Generic;
using System.Linq;
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
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.AgentStatus
{
    internal sealed class TeamCityAgentStatusTileViewModel : ConfiguredTileViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();
        private readonly IServices _services;
        private IBuildAgent _buildAgent = new NullBuildAgent("");
        private static int _nextInstanceId = 1;

        public TeamCityAgentStatusTileViewModel(ITeamCityChannel channel, TileConfiguration tileConfiguration, IDashboardController dashboardController, ITileLayoutController tileLayoutController, IServices services, ITeamCityAgentStatusTileControl view, ITileProperties tileProperties)
            : base(tileProperties)
        {
            lock (_syncRoot)
            {
                _logger = LogManager.GetLogger($"Tiles.TeamCity.Agent.{_nextInstanceId++}");
            }

            _services = services;
            _tileConfigurationConverter = new TileConfigurationConverter(tileConfiguration, this);
            ConfigureServiceCommand = new DataSourceConfigureCommand(channel);

            var configurationParameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tileConfiguration, "Build Agent Configuration", configurationParameters, dashboardController, tileLayoutController, services);

            Subscribe();

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

        public void OnConfigurationChanged(INamedValueRepository converter)
        {
            _logger.Debug("Configuration changed.");
            GetBuildAgent();
        }

        private void Subscribe()
        {
            // todo - change to use subscription model
        }

        private void GetBuildAgent()
        {
            var name = _tileConfigurationConverter.GetString("Agent");

            // todo - move this is a teamcity service aggregator
            foreach (var service in _services.GetServicesOfType<ITeamCityService>())
            {
                if (service.Agents.Has(name))
                {
                    BuildAgent = service.Agents.Get(name);
                }
            }
        }

        private IEnumerable<IPropertyViewModel> GetConfigurationParameters()
        {
            var configurationParameters = new IPropertyViewModel[]
            {
                new TextPropertyViewModel("Agent", _tileConfigurationConverter, _services)
            };
            return configurationParameters;
        }

        private ICommand ConfigureServiceCommand { get; }
    }
}