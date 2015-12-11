using System;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.TeamCity.AgentStatus
{
    public sealed class TeamCityAgentStatusTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly TeamCityService _service;
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public TeamCityAgentStatusTilePlugin(TeamCityService service, IDashboardController dashboardController, IServices services)
        {
            _service = service;
            _dashboardController = dashboardController;
            _services = services;
        }

        public int Rank => 0;

        public string Name => "TeamCity build agent status";

        public bool MatchesId(string id)
        {
            return id == TeamCityAgentTileController.TileTypeId;
        }

        public IViewController CreateTileController(TileConfiguration tile, TileLayoutController layoutController)
        {
            return new TeamCityAgentTileController(_service, tile, _dashboardController, layoutController, _services);
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TeamCityAgentTileController.TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }

        public void Register(IServices services)
        {
            services.TileProviders.Register(this);
        }
    }
}