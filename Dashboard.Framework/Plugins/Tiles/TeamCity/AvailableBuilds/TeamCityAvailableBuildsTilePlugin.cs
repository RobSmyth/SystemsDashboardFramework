using log4net;
using NoeticTools.SystemsDashboard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.AvailableBuilds
{
    public sealed class TeamCityLAvailbleBuildSTilePlugin : IPlugin
    {
        private ILog _logger;

        public TeamCityLAvailbleBuildSTilePlugin()
        {
            _logger = LogManager.GetLogger("Plugin.TeamCity.AvailableBuilds");
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new TeamCityLAvailbleBuildSTileProvider(services.GetService<TeamCityService>("TeamCity"), services.DashboardController, services));
        }
    }
}