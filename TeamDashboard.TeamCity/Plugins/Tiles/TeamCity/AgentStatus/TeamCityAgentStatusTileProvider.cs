using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.AgentStatus;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.Tiles.TeamCity.AgentStatus
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

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new TeamCityAgentStatusTileControl();
            new TeamCityAgentStatusTileViewModel(_channel, tile, _dashboardController, layoutController, _services, view, _channel);
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