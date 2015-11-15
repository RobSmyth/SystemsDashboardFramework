using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.Dashboard.Framework.DataSources.Jira;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.Dashboard.Framework.DataSources.TeamCity
{
    internal class TeamCityChannelConnectedState : ITeamCityChannel
    {
        private readonly TeamCityClient _client;
        private readonly IStateEngine<ITeamCityChannel> _stateEngine;
        private readonly IClock _clock;
        private readonly TimeRefreshedItems<Project> _projects;
        private readonly Dictionary<Project, TimeRefreshedItems<BuildConfig>> _buildConfigurations = new Dictionary<Project, TimeRefreshedItems<BuildConfig>>();

        public TeamCityChannelConnectedState(TeamCityClient client, IStateEngine<ITeamCityChannel> stateEngine, IClock clock)
        {
            _client = client;
            _stateEngine = stateEngine;
            _clock = clock;
            _projects = new TimeRefreshedItems<Project>(() => _client.Projects.All(), TimeSpan.FromMinutes(5), clock);
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
                var project = _projects.Items.Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                var buildConfiguration = GetConfiguration(project, buildConfigurationName);
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
                var project = _projects.Items.Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                var buildConfiguration = _client.BuildConfigs.ByProjectIdAndConfigurationName(project.Id, buildConfigurationName);
                var builds = _client.Builds.SuccessfulBuildsByBuildConfigId(buildConfiguration.Id);
                return builds.FirstOrDefault();
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
                var project = _projects.Items.Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                var buildConfiguration = GetConfiguration(project, buildConfigurationName);
                var builds = _client.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true));
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
                var builds = _client.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: branchName));
                return builds.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private BuildConfig GetConfiguration(Project project, string buildConfigurationName)
        {
            if (!_buildConfigurations.ContainsKey(project))
            {
                _buildConfigurations.Add(project, new TimeRefreshedItems<BuildConfig>(() => _client.BuildConfigs.ByProjectId(project.Id), TimeSpan.FromSeconds(30), _clock));
            }
            return _buildConfigurations[project].Items.Single(x => x.Name.Equals(buildConfigurationName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}