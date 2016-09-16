using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity;


namespace NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.AgentStatus
{
    public sealed class TeamCityAgentStatusTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new TeamCityAgentStatusTileProvider(services.GetService<ITeamCityService>("TeamCity").Channel, services.DashboardController, services));
        }
    }
}