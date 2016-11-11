using System.Collections.Generic;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TestDataSource;
using NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.AgentStatus;
using NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.AvailableBuilds;
using NoeticTools.TeamStatusBoard.TeamCity.Tiles.TeamCity.LastBuildStatus;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity
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