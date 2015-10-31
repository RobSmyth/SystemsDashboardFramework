using System;
using System.Collections.Generic;
using System.Linq;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace NoeticTools.Dashboard.Framework.DataSources.TeamCity
{
    internal class TeamCityChannelConnectedState : ITeamCityChannel
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
                Project project =
                    _client.Projects.All()
                        .Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                BuildConfig buildConfiguration = _client.BuildConfigs.ByProjectIdAndConfigurationName(project.Id,
                    buildConfigurationName);
                List<Build> builds = _client.Builds.ByBuildConfigId(buildConfiguration.Id);
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
                Project project =
                    _client.Projects.All()
                        .Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                BuildConfig buildConfiguration = _client.BuildConfigs.ByProjectIdAndConfigurationName(project.Id,
                    buildConfigurationName);
                List<Build> builds = _client.Builds.SuccessfulBuildsByBuildConfigId(buildConfiguration.Id);
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
                Project project =
                    _client.Projects.All()
                        .Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                BuildConfig buildConfiguration = _client.BuildConfigs.ByProjectIdAndConfigurationName(project.Id,
                    buildConfigurationName);
                List<Build> builds =
                    _client.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: false,
                        status: BuildStatus.SUCCESS, branch: branchName));
                //var builds = _client.Builds.ByBranch(branchName);
                return builds.FirstOrDefault( /*x => x.BuildConfig.Id == buildConfiguration.Id*/);
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
                Project project =
                    _client.Projects.All()
                        .Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                BuildConfig buildConfiguration = _client.BuildConfigs.ByProjectIdAndConfigurationName(project.Id,
                    buildConfigurationName);
                List<Build> builds = _client.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true));
                return builds.FirstOrDefault(x => x.Status != "UNKNOWN" && x.WebUrl.EndsWith(buildConfiguration.Id));
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
                List<Build> builds =
                    _client.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: branchName));
                return builds.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}