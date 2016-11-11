using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TestDataSource;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.Tiles.TeamCity.AgentStatus;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.Tiles.TeamCity.AvailableBuilds;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.Tiles.TeamCity.LastBuildStatus;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity
{
    public sealed class TeamCityPlugin : IPlugin
    {
        public int Rank => 90;

        public void Register(IServices services)
        {
            new TeamCityDataSourcePlugin("TeamCity").Register(services);
            new TeamCityTestDataSourcePlugin("TestData_TeamCity").Register(services);

            new TeamCityAgentStatusTilePlugin("TeamCity").Register(services);
            new TeamCityLastBuildStatusTilePlugin("TeamCity").Register(services);
            new TeamCityAvailbleBuildSTilePlugin("TeamCity").Register(services);
        }
    }
}