using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.Tiles.TeamCity.AgentStatus
{
    public sealed class TeamCityAgentStatusTilePlugin : IPlugin
    {
        private readonly string _serviceName;

        public TeamCityAgentStatusTilePlugin(string serviceName)
        {
            _serviceName = serviceName;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new TeamCityAgentStatusTileProvider(services.DashboardController, services));
        }
    }
}