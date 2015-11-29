using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.TeamCity.LastBuildStatus
{
    public class TeamCityLastBuildStatusTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly TeamCityService _service;
        private readonly ITimerService _timerService;
        private readonly IDashboardController _dashboardController;

        public TeamCityLastBuildStatusTilePlugin(TeamCityService service, ITimerService timerService, IDashboardController dashboardController)
        {
            _service = service;
            _timerService = timerService;
            _dashboardController = dashboardController;
        }

        public string Name => "TeamCity build status";

        public bool MatchesId(string id)
        {
            return id == TeamCityLastBuildStatusTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B5", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateTileController(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController)
        {
            return new TeamCityLastBuildStatusTileController(_service, tileConfiguration, _timerService, _dashboardController, tileLayoutController);
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TeamCityLastBuildStatusTileController.TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }

        public void Register(IServices services)
        {
            services.TileProviderRegistry.Register(this);
        }
    }
}