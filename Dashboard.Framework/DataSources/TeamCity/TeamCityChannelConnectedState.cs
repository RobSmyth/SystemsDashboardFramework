using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using NoeticTools.SystemsDashboard.Framework.DataSources.Jira;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    internal class TeamCityChannelConnectedState : ITeamCityChannel
    {
        private readonly TeamCityClient _client;
        private readonly IClock _clock;
        private readonly TimeCachedArray<Project> _projects;
        private readonly Dictionary<Project, TimeCachedArray<BuildConfig>> _buildConfigurations = new Dictionary<Project, TimeCachedArray<BuildConfig>>();
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();

        public TeamCityChannelConnectedState(TeamCityClient client, IClock clock)
        {
            _client = client;
            _clock = clock;
            _logger = LogManager.GetLogger("DateSources.TeamCity.Connected");
            _projects = new TimeCachedArray<Project>(() => _client.Projects.All(), TimeSpan.FromMinutes(5), clock);
        }

        public string[] ProjectNames => _projects.Items.Select(x => x.Name).ToArray();
        public bool IsConnected => true;

        public void Connect()
        {
        }

        public async Task<string[]> GetConfigurationNames(string projectName)
        {
            _logger.DebugFormat("Request for configuration names for project {0}", projectName);

            var project = _projects.Items.SingleOrDefault(x => x.Name.Equals(projectName, StringComparison.CurrentCultureIgnoreCase));
            var configurations = await GetConfigurations(project);
            return project == null ? new string[0] : configurations.Items.Select(x => x.Name).ToArray();
        }

        public async Task<Build> GetLastBuild(string projectName, string buildConfigurationName)
        {
            _logger.DebugFormat("Request for last build: {0} / {1}.", projectName, buildConfigurationName);

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
            _logger.DebugFormat("Request for last successful build: {0} / {1}.", projectName, buildConfigurationName);

            return Task.Run(() =>
            {
                lock (_syncRoot)
                {
                    try
                    {
                        var project = _projects.Items.Single(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                        var buildConfiguration = _client.BuildConfigs.ByProjectIdAndConfigurationName(project.Id, buildConfigurationName);
                        var builds = _client.Builds.SuccessfulBuildsByBuildConfigId(buildConfiguration.Id);
                        var lastBuild = builds.FirstOrDefault();
                        if (lastBuild == null)
                        {
                            _logger.WarnFormat("Could not find a last successful build for: {0} / {1}.", projectName, buildConfigurationName);
                        }
                        else
                        {
                            _logger.DebugFormat("Last successful build was run at {2} for: {0} / {1}.", projectName, buildConfigurationName, lastBuild.StartDate);
                        }
                        return lastBuild;
                    }
                    catch (Exception exception)
                    {
                        _logger.Error("Exception getting last successful build.", exception);
                        return null;
                    }
                }
            });
        }

        public async Task<Build> GetRunningBuild(string projectName, string buildConfigurationName)
        {
            _logger.DebugFormat("Request for running build: {0} / {1}.", projectName, buildConfigurationName);

            try
            {
                var project = _projects.Items.SingleOrDefault(x => x.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase));
                if (project == null)
                {
                    _logger.WarnFormat("Could not find project {0}.", projectName);
                    return null;
                }

                var buildConfiguration = await GetConfiguration(project, buildConfigurationName);
                if (buildConfiguration == null)
                {
                    _logger.WarnFormat("Could not find configuration: {0} / {1}.", projectName, buildConfigurationName);
                    return null;
                }

                var builds = _client.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true));
                var runningBuild = builds.FirstOrDefault(x => x.Status != "UNKNOWN" && x.WebUrl.EndsWith(buildConfiguration.Id)) ?? null;
                if (runningBuild == null)
                {
                    _logger.DebugFormat("No build running: {0} / {1}.", projectName, buildConfigurationName);
                }
                else
                {
                    _logger.DebugFormat("Build running: {0} / {1}.", projectName, buildConfigurationName);
                    runningBuild.Status = runningBuild.Status == "FAILED" ? "RUNNING FAILED" : "RUNNING";
                }

                return runningBuild;
            }
            catch (Exception exception)
            {
                _logger.Error("Exception while getting running build.", exception);
                return null;
            }
        }

        public Task<Build> GetRunningBuild(string projectName, string buildConfigurationName, string branchName)
        {
            _logger.DebugFormat("Request for last build on branch {2}: {0} / {1}.", projectName, buildConfigurationName, branchName);

            return Task.Run(() =>
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
            });
        }

        private async Task<BuildConfig> GetConfiguration(Project project, string buildConfigurationName)
        {
            var buildConfigurations = await GetConfigurations(project);
            return buildConfigurations.Items.SingleOrDefault(x => x.Name.Equals(buildConfigurationName, StringComparison.InvariantCultureIgnoreCase));
        }

        private Task<TimeCachedArray<BuildConfig>> GetConfigurations(Project project)
        {
            _logger.DebugFormat("Request for configurations on project {0}.", project.Name);

            return Task.Run(() =>
            {
                lock (_syncRoot)
                {
                    if (!_buildConfigurations.ContainsKey(project))
                    {
                        var timeCachedArray = new TimeCachedArray<BuildConfig>(() =>
                        {
                            var byProjectId = _client.BuildConfigs.ByProjectId(project.Id);
                            return byProjectId;
                        }, TimeSpan.FromSeconds(30), _clock);

                        _buildConfigurations.Add(project, timeCachedArray);
                    }
                    return _buildConfigurations[project];
                }
            });
        }
    }
}