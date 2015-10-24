using System;
using System.Linq;
using Dashboard.TeamCity;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace Dashboard.Tiles.TeamCity
{
    class TeamCityChannelConnectedState : ITeamCityChannel
    {
        private readonly TeamCityClient _client;
        private readonly IStateEngine<ITeamCityChannel> _stateEngine;

        public TeamCityChannelConnectedState(TeamCityClient client, IStateEngine<ITeamCityChannel> stateEngine)
        {
            _client = client;
            _stateEngine = stateEngine;
        }

        public void Connect()
        {
        }

        public void Disconnect()
        {
            _stateEngine.OnDisconnected();
        }

        public Build GetLastBuild(string projectName, string buildConfigurationName)
        {
            try
            {
                var project = _client.Projects.All().Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                var buildConfiguration = _client.BuildConfigs.ByProjectIdAndConfigurationName(project.Id, buildConfigurationName);
                var builds = _client.Builds.ByBuildConfigId(buildConfiguration.Id);
                return builds.FirstOrDefault(x => x.Status != "UNKNOWN");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            try
            {
                var project = _client.Projects.All().Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                var buildConfiguration = _client.BuildConfigs.ByProjectIdAndConfigurationName(project.Id, buildConfigurationName);
                var builds = _client.Builds.SuccessfulBuildsByBuildConfigId(buildConfiguration.Id);
                return builds.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName, string branchName)
        {
            try
            {
                var project = _client.Projects.All().Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                var buildConfiguration = _client.BuildConfigs.ByProjectIdAndConfigurationName(project.Id, buildConfigurationName);
                var builds = _client.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: false, status: BuildStatus.SUCCESS, branch: branchName));
                //var builds = _client.Builds.ByBranch(branchName);
                return builds.FirstOrDefault(/*x => x.BuildConfig.Id == buildConfiguration.Id*/);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Build GetRunningBuild(string projectName, string buildConfigurationName)
        {
            try
            {
                var project = _client.Projects.All().Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                var buildConfiguration = _client.BuildConfigs.ByProjectIdAndConfigurationName(project.Id, buildConfigurationName);
                var builds = _client.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true));
                return builds.FirstOrDefault(x => x.Status != "UNKNOWN");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Build GetRunningBuild(string projectName, string buildConfigurationName, string branchName)
        {
            try
            {
                var builds = _client.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: branchName));
                return builds.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
