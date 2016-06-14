using log4net;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.Tiles.TeamCity.AvailableBuilds
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
            var teamCityDataSourceChannel = services.GetService<ITeamCityService>("TeamCity").Channel;
            services.TileProviders.Register(new TeamCityAvailbleBuildsTileProvider(teamCityDataSourceChannel, services.DashboardController, services));
        }
    }
}