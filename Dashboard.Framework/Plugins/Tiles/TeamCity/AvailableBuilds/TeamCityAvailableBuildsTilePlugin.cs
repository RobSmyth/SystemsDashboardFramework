using log4net;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.TeamCity.AvailableBuilds
{
    public sealed class TeamCityLAvailbleBuildSTilePlugin : IPlugin
    {
        private readonly TeamCityService _service;
        private readonly IDashboardController _dashboardController;
        private ILog _logger;

        public TeamCityLAvailbleBuildSTilePlugin(TeamCityService service, IDashboardController dashboardController)
        {
            _service = service;
            _dashboardController = dashboardController;
            _logger = LogManager.GetLogger("Plugin.TeamCity.AvailableBuilds");
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new TeamCityLAvailbleBuildSTileProvider(_service, _dashboardController, services));
        }
    }
}