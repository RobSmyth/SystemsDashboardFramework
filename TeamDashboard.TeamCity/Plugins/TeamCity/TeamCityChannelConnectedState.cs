using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using NoeticTool.sTeamStatusBoard.TeamCity.Plugins.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.DataSources.Jira;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    internal class TeamCityChannelConnectedState : ITeamCityChannel
    {
        private readonly TeamCityClient _teamCityClient;
        private readonly IStateEngine<ITeamCityChannel> _stateEngine;
        private readonly IClock _clock;
        private readonly IBuildAgentRepository _buildAgentRepository;
        private readonly IServices _services;
        private readonly IDataSource _dataSource;
        private IProjectRepository _projectRepository;
        private readonly Dictionary<Project, ITimeCachedArray<BuildConfig>> _buildConfigurations = new Dictionary<Project, ITimeCachedArray<BuildConfig>>();
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();

        public TeamCityChannelConnectedState(TeamCityClient teamCityClient, IStateEngine<ITeamCityChannel> stateEngine, IBuildAgentRepository buildAgentRepository, IServices services, IDataSource dataSource)
        {
            _teamCityClient = teamCityClient;
            _stateEngine = stateEngine;
            _clock = services.Clock;
            _buildAgentRepository = buildAgentRepository;
            _services = services;
            _dataSource = dataSource;
            _logger = LogManager.GetLogger("DateSources.TeamCity.Connected");
        }

        public string[] ProjectNames => _projectRepository.GetAll().Select(x => x.Name).ToArray();

        public bool IsConnected => true;

        public void Connect()
        {
        }

        public void Disconnect()
        {
            _stateEngine.OnDisconnected();
        }

        public async Task<string[]> GetConfigurationNames(string projectName)
        {
            _logger.DebugFormat("Request for configuration names for project {0}", projectName);

            var project = _projectRepository.Get(projectName);
            var configurations = await GetConfigurations(project.Inner);
            return project == null ? new string[0] : configurations.Items.Select(x => x.Name).ToArray();
        }

        public Task<IBuildAgent> GetAgent(string name)
        {
            return Task.Run(() =>
            {
                if (!_buildAgentRepository.Has(name))
                {
                    _buildAgentRepository.Add(new TeamCityBuildAgentViewModel(name, _teamCityClient, _services.Timer));
                }
                return _buildAgentRepository.Get(name);
            });
        }

        public Task<IBuildAgent[]> GetAgents()
        {
            return Task.Run(() =>
            {
                UpdateBuildAgentRepository();
                var agents = _buildAgentRepository.GetAll();

                return agents;
            });
        }

        public async Task<Build> GetLastBuild(string projectName, string buildConfigurationName)
        {
            _logger.DebugFormat("Request for last build: {0} / {1}.", projectName, buildConfigurationName);

            try
            {
                var project = _projectRepository.Get(projectName).Inner;
                if (project == null)
                {
                    return null;
                }
                var buildConfiguration = await GetConfiguration(project, buildConfigurationName);
                if (buildConfiguration == null)
                {
                    return null;
                }
                var builds = _teamCityClient.Builds.ByBuildConfigId(buildConfiguration.Id);
                var build = builds.FirstOrDefault(x => x.Status != "UNKNOWN");
                return build;
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
                        var project = _projectRepository.Get(projectName).Inner;
                        var buildConfiguration = _teamCityClient.BuildConfigs.ByProjectIdAndConfigurationName(project.Id, buildConfigurationName);
                        var builds = _teamCityClient.Builds.SuccessfulBuildsByBuildConfigId(buildConfiguration.Id);
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

        public async Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName)
        {
            _logger.DebugFormat("Request for running build: {0} / {1}.", projectName, buildConfigurationName);

            try
            {
                var project = _projectRepository.Get(projectName).Inner;
                if (project == null)
                {
                    _logger.WarnFormat("Could not find project {0}.", projectName);
                    return new Build[0];
                }

                var buildConfiguration = await GetConfiguration(project, buildConfigurationName);
                if (buildConfiguration == null)
                {
                    _logger.WarnFormat("Could not find configuration: {0} / {1}.", projectName, buildConfigurationName);
                    return new Build[0];
                }

                var builds = _teamCityClient.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: "default:any")).Where(x => x.WebUrl.EndsWith(buildConfiguration.Id)).ToArray();

                foreach (var build in builds)
                {
                    build.Status = build.Status == "FAILED" ? "RUNNING FAILED" : "RUNNING";
                }

                return builds;
            }
            catch (Exception exception)
            {
                _logger.Error("Exception while getting running build.", exception);
                return null;
            }
        }

        public Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName, string branchName)
        {
            _logger.DebugFormat("Request for last build on branch {2}: {0} / {1}.", projectName, buildConfigurationName, branchName);

            return Task.Run(() =>
            {
                try
                {
                    var builds = _teamCityClient.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: branchName));
                    return builds.ToArray();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public void Enter()
        {
            var projectCache = new TimeCachedArray<Project>(() => _teamCityClient.Projects.All(), TimeSpan.FromMinutes(5), _services.Clock);
            _projectRepository = new ProjectRepository(_dataSource, projectCache);

            Task.Run(() => GetAgents());
            if (ProjectNames.Length > 0)
            {
                Task.Run(() => GetConfigurationNames(ProjectNames.First()));
            }
        }

        private void UpdateBuildAgentRepository()
        {
            var teamCityAgents = _teamCityClient.Agents.All();
            foreach (var teamCityAgent in teamCityAgents)
            {
                if (!_buildAgentRepository.Has(teamCityAgent.Name))
                {
                    _buildAgentRepository.Add(new TeamCityBuildAgentViewModel(teamCityAgent.Name, _teamCityClient, _services.Timer));
                }
            }
        }

        private async Task<BuildConfig> GetConfiguration(Project project, string buildConfigurationName)
        {
            var buildConfigurations = await GetConfigurations(project);
            return buildConfigurations.Items.SingleOrDefault(x => x.Name.Equals(buildConfigurationName, StringComparison.InvariantCultureIgnoreCase));
        }

        private Task<ITimeCachedArray<BuildConfig>> GetConfigurations(Project project)
        {
            _logger.DebugFormat("Request for configurations on project {0}.", project.Name);

            return Task.Run(() =>
            {
                lock (_syncRoot)
                {
                    if (!_buildConfigurations.ContainsKey(project))
                    {
                        var timeCachedArray = new TimeCachedArray<BuildConfig>(() => _teamCityClient.BuildConfigs.ByProjectId(project.Id), TimeSpan.FromHours(12), _clock);
                        _buildConfigurations.Add(project, timeCachedArray);
                    }
                    return _buildConfigurations[project];
                }
            });
        }
    }
}