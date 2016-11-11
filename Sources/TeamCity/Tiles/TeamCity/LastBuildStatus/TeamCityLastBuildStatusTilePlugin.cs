using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.Tiles.TeamCity.LastBuildStatus
{
    public sealed class TeamCityLastBuildStatusTilePlugin : IPlugin
    {
        private readonly string _serviceName;

        public TeamCityLastBuildStatusTilePlugin(string serviceName)
        {
            // todo - should not be passing in service name here. Breaks having multiple team city services.
            _serviceName = serviceName;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new TeamCityLastBuildStatusTileProvider(services.GetService<ITeamCityService>(_serviceName).Channel, services.DashboardController, services));
        }
    }
}