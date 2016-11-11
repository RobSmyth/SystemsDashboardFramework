using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Styles;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.AgentStatus
{
    public sealed class TeamCityAgentStatusTileProvider : ITileControllerProvider
    {
        private const string TileTypeId = "TeamCity.Agent.Status";
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public TeamCityAgentStatusTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "TeamCity build agent status";

        public string TypeId => TeamCityAgentStatusTileProvider.TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            // todo - do not get channel here, breaks need for multiple teamcity services
            var channel =_services.GetService<ITeamCityService>("TeamCity").Channel;
            var conduit = new ConfigurationChangeListenerConduit();
            var tileProperties = new TileProperties(tileConfigturation, conduit, _services);
            var view = new TeamCityAgentStatusTileControl();
            new StatusBoardStyleStategy(view, _services.Style);
            var viewModel = new TeamCityAgentStatusTileViewModel(channel, tileConfigturation, _dashboardController, layoutController, _services, view, tileProperties);
            conduit.SetTarget(viewModel);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TileTypeId,
            };
        }
    }
}