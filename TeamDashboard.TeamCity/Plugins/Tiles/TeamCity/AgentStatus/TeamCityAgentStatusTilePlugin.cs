using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.Tiles.TeamCity.AgentStatus
{
    public sealed class TeamCityAgentStatusTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new TeamCityAgentStatusTileProvider(services.GetService<TeamCityService>("TeamCity"), services.DashboardController, services));
        }
    }
}