using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
        private readonly TimeCachedArray<Project> _projects;
        private readonly Dictionary<Project, TimeCachedArray<BuildConfig>> _buildConfigurations = new Dictionary<Project, TimeCachedArray<BuildConfig>>();

        public TeamCityChannelConnectedState(TeamCityClient client, IStateEngine<ITeamCityChannel> stateEngine, IClock clock)
        {
            _client = client;
            _stateEngine = stateEngine;
            _clock = clock;
            _projects = new TimeCachedArray<Project>(() => _client.Projects.All(), TimeSpan.FromMinutes(5), clock);
        }

        public void Connect()
        {
        }

        public void Disconnect()
        {
            _stateEngine.OnDisconnected();
        }

        public string[] GetConfigurationNames(string projectName)
        {
            var project = _projects.Items.Single(x => x.Name.Equals(projectName, StringComparison.CurrentCultureIgnoreCase));
            return project == null ? new string[0] : GetConfigurations(project).Items.Select(x => x.Name).ToArray();
        }

        public string[] ProjectNames => _projects.Items.Select(x => x.Name).ToArray();

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
            var buildConfigurations = GetConfigurations(project);
            return buildConfigurations.Items.Single(x => x.Name.Equals(buildConfigurationName, StringComparison.InvariantCultureIgnoreCase));
        }

        private TimeCachedArray<BuildConfig> GetConfigurations(Project project)
        {
            if (!_buildConfigurations.ContainsKey(project))
            {
                _buildConfigurations.Add(project, new TimeCachedArray<BuildConfig>(() => _client.BuildConfigs.ByProjectId(project.Id), TimeSpan.FromSeconds(30), _clock));
            }
            var buildConfigurations = _buildConfigurations[project];
            return buildConfigurations;
        }
    }
}