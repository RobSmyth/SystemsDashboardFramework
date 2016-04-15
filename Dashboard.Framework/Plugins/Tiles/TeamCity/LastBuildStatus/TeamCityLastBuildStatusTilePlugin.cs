using NoeticTools.SystemsDashboard.Framework.Plugins;
using NoeticTools.SystemsDashboard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.LastBuildStatus
{
    public sealed class TeamCityLastBuildStatusTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new TeamCityLastBuildStatusTileProvider(services.GetService<TeamCityService>("TeamCity"), services.DashboardController, services));
        }
    }
}