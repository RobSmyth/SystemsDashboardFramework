using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.LastBuildStatus
{
    public sealed class TeamCityLastBuildStatusTilePlugin : IPlugin
    {
        private readonly string _serviceName;

        public TeamCityLastBuildStatusTilePlugin(string serviceName)
        {
            _serviceName = serviceName;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new TeamCityLastBuildStatusTileProvider(services.GetService<ITeamCityService>(_serviceName).Channel, services.DashboardController, services));
        }
    }
}