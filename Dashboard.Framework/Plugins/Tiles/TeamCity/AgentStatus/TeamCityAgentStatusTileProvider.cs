using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.AgentStatus
{
    public sealed class TeamCityAgentStatusTileProvider : ITileControllerProvider
    {
        private const string TileTypeId = "TeamCity.Agent.Status";
        private readonly TeamCityService _service;
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public TeamCityAgentStatusTileProvider(TeamCityService service, IDashboardController dashboardController, IServices services)
        {
            _service = service;
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
            new TeamCityAgentStatusTileViewModel(_service, tile, _dashboardController, layoutController, _services, view, _service);
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