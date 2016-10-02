using System.Collections.Generic;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
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
            Register(services, "TeamCity");
            RegisterTestService(services, "TestData_TeamCity");
        }

        private static void RegisterTestService(IServices services, string testServiceName)
        {
            Register(services, testServiceName);
            new TeamCityTestDataPlugin(testServiceName).Register(services);
        }

        private static void Register(IServices services, string serviceName)
        {
            foreach (var plugin in GetTeamCityServicePlugins(serviceName))
            {
                plugin.Register(services);
            }
        }

        private static IEnumerable<IPlugin> GetTeamCityServicePlugins(string serviceName)
        {
            return new IPlugin[]
            {
                new TeamCityDataSourcePlugin(serviceName),
                new TeamCityAgentStatusTilePlugin(serviceName),
                new TeamCityLastBuildStatusTilePlugin(serviceName),
                new TeamCityLAvailbleBuildSTilePlugin(serviceName),
            };
        }
    }
}