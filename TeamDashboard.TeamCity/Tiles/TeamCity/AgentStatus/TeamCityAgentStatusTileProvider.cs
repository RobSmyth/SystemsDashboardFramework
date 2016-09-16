using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.AgentStatus
{
    public sealed class TeamCityAgentStatusTileProvider : ITileControllerProvider
    {
        private const string TileTypeId = "TeamCity.Agent.Status";
        private readonly ITeamCityChannel _channel;
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public TeamCityAgentStatusTileProvider(ITeamCityChannel channel, IDashboardController dashboardController, IServices services)
        {
            _channel = channel;
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "TeamCity build agent status (future)";

        public string TypeId => TeamCityAgentStatusTileProvider.TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            var teamCityService = _services.GetService<ITeamCityService>("TeamCity");
            var view = new TeamCityAgentStatusTileControl();
            new TeamCityAgentStatusTileViewModel(_channel, tileConfigturation, _dashboardController, layoutController, _services, view, teamCityService);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }
    }
}