using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.DataSources.Jira;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    internal sealed class TeamCityChannelConnectedState : ITeamCityChannelState
    {
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly IStateEngine<ITeamCityChannel> _stateEngine;
        private readonly IProjectRepository _projectRepository;
        private readonly IClock _clock;
        private readonly IBuildAgentRepository _buildAgentRepository;
        private readonly IServices _services;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly Dictionary<IProject, TimeCachedArray<BuildConfig>> _buildConfigurations = new Dictionary<IProject, TimeCachedArray<BuildConfig>>();
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();

        public TeamCityChannelConnectedState(ITcSharpTeamCityClient teamCityClient, IStateEngine<ITeamCityChannel> stateEngine,
            IProjectRepository projectRepository, IBuildAgentRepository buildAgentRepository, IServices services, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _teamCityClient = teamCityClient;
            _stateEngine = stateEngine;
            _projectRepository = projectRepository;
            _clock = services.Clock;
            _buildAgentRepository = buildAgentRepository;
            _services = services;
            _channelStateBroadcaster = channelStateBroadcaster;
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
            var configurations = await GetConfigurations(project);
            return project == null ? new string[0] : configurations.Items.Select(x => x.Name).ToArray();
        }

        public Task<IBuildAgent> GetAgent(string name)
        {
            return Task.Run(() => _buildAgentRepository.Get(name));
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

        public Build GetLastBuild(string projectName, string buildConfigurationName)
        {
            _logger.DebugFormat("Request for last build: {0} / {1}.", projectName, buildConfigurationName);

            try
            {
                var project = _projectRepository.Get(projectName);
                var buildConfiguration = GetConfiguration(project, buildConfigurationName);
                if (buildConfiguration == null)
                {
                    return null;
                }
                var builds = _teamCityClient.Builds.ByBuildConfigId(buildConfiguration.Result.Id);
                return builds.FirstOrDefault(x => x.Status != "UNKNOWN");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            _logger.DebugFormat("Request for last successful build: {0} / {1}.", projectName, buildConfigurationName);

            lock (_syncRoot)
            {
                try
                {
                    var project = _projectRepository.Get(projectName);
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
        }

        public Build[] GetRunningBuilds(string projectName, string buildConfigurationName)
        {
            _logger.DebugFormat("Request for running build: {0} / {1}.", projectName, buildConfigurationName);

            try
            {
                return _projectRepository.Get(projectName).GetRunningBuilds(buildConfigurationName);
            }
            catch (Exception exception)
            {
                _logger.Error("Exception while getting running build.", exception);
                return new Build[0];
            }
        }

        public Build[] GetRunningBuilds(string projectName, string buildConfigurationName, string branchName)
        {
            _logger.DebugFormat("Request for last build on branch {2}: {0} / {1}.", projectName, buildConfigurationName, branchName);

            try
            {
                var builds = _teamCityClient.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: branchName));
                return builds.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        void ITeamCityChannelState.Leave()
        {
        }

        void ITeamCityChannelState.Enter()
        {
            Task.Run(GetAgents);
            if (ProjectNames.Length > 0)
            {
                Task.Run(() => GetConfigurationNames(ProjectNames.First()));
            }
            _channelStateBroadcaster.OnConnected.Fire();
        }

        private void UpdateBuildAgentRepository()
        {
            var teamCityAgents = _teamCityClient.Agents.All();
            foreach (var teamCityAgent in teamCityAgents)
            {
                _buildAgentRepository.Get(teamCityAgent.Name); // todo - move this functionality to the repository
            }
        }

        private async Task<BuildConfig> GetConfiguration(IProject project, string buildConfigurationName)
        {
            var buildConfigurations = await GetConfigurations(project);
            return buildConfigurations.Items.SingleOrDefault(x => x.Name.Equals(buildConfigurationName, StringComparison.InvariantCultureIgnoreCase));
        }

        private Task<TimeCachedArray<BuildConfig>> GetConfigurations(IProject project)
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