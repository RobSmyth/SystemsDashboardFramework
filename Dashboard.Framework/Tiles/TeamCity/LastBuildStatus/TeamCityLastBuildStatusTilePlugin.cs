using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Tiles.Date;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Tiles.TeamCity.LastBuildStatus
{
    public class TeamCityLastBuildStatusTilePlugin : IPlugin, IViewControllerProvider
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

        public bool MatchesId(string id)
        {
            return id == TeamCityLastBuildStatusTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B5", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateViewController(TileConfiguration tileConfiguration)
        {
            return new TeamCityLastBuildStatusTileController(_service, tileConfiguration, _timerService, _dashboardController);
        }

        public void Register(IServices services)
        {
            services.Repository.Register(this);
        }
    }
}