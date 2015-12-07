using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoeticTools.SystemsDashboard.Framework.DataSources.Jira;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    internal class TeamCityChannelConnectedState : ITeamCityChannel
    {
        private readonly Build _nullBuild = new NullBuild();
        private readonly TeamCityClient _client;
        private readonly IClock _clock;
        private readonly TimeCachedArray<Project> _projects;
        private readonly Dictionary<Project, TimeCachedArray<BuildConfig>> _buildConfigurations = new Dictionary<Project, TimeCachedArray<BuildConfig>>();

        public TeamCityChannelConnectedState(TeamCityClient client, IClock clock)
        {
            _client = client;
            _clock = clock;
            _projects = new TimeCachedArray<Project>(() => _client.Projects.All(), TimeSpan.FromMinutes(5), clock);
        }

        public string[] ProjectNames => _projects.Items.Select(x => x.Name).ToArray();

        public void Connect()
        {
        }

        public async Task<string[]> GetConfigurationNames(string projectName)
        {
            var project = _projects.Items.SingleOrDefault(x => x.Name.Equals(projectName, StringComparison.CurrentCultureIgnoreCase));
            var configurations = await GetConfigurations(project);
            return project == null ? new string[0] : configurations.Items.Select(x => x.Name).ToArray();
        }

        public async Task<Build> GetLastBuild(string projectName, string buildConfigurationName)
        {
            try
            {
                var project = _projects.Items.SingleOrDefault(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                if (project == null)
                {
                    return null;
                }
                var buildConfiguration = await GetConfiguration(project, buildConfigurationName);
                if (buildConfiguration == null)
                {
                    return null;
                }
                var builds = _client.Builds.ByBuildConfigId(buildConfiguration.Id);
                return builds.FirstOrDefault(x => x.Status != "UNKNOWN");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<Build> GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            return Task.Run(() =>
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
            });
        }

        public async Task<Build> GetRunningBuild(string projectName, string buildConfigurationName)
        {
            try
            {
                var project = _projects.Items.SingleOrDefault(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                if (project == null)
                {
                    return _nullBuild;
                }
                var buildConfiguration = await GetConfiguration(project, buildConfigurationName);
                var builds = _client.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true));
                return builds.FirstOrDefault(x => x.Status != "UNKNOWN" && x.WebUrl.EndsWith(buildConfiguration.Id)) ?? _nullBuild;
            }
            catch (Exception)
            {
                return _nullBuild;
            }
        }

        public Task<Build> GetRunningBuild(string projectName, string buildConfigurationName, string branchName)
        {
            return Task.Run(() =>
            {
                try
                {
                    var builds = _client.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: branchName));
                    return builds.FirstOrDefault();
                }
                catch (Exception)
                {
                    return _nullBuild;
                }
            });
        }

        private async Task<BuildConfig> GetConfiguration(Project project, string buildConfigurationName)
        {
            var buildConfigurations = await GetConfigurations(project);
            return buildConfigurations.Items.SingleOrDefault(x => x.Name.Equals(buildConfigurationName, StringComparison.InvariantCultureIgnoreCase));
        }

        private Task<TimeCachedArray<BuildConfig>> GetConfigurations(Project project)
        {
            return Task.Run(() =>
            {
                if (!_buildConfigurations.ContainsKey(project))
                {
                    _buildConfigurations.Add(project, new TimeCachedArray<BuildConfig>(() => _client.BuildConfigs.ByProjectId(project.Id), TimeSpan.FromSeconds(30), _clock));
                }
                var buildConfigurations = _buildConfigurations[project];
                return buildConfigurations;
            });
        }
    }
}