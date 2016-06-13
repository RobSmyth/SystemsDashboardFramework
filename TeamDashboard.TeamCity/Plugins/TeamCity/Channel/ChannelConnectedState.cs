using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.DataSources.Jira;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Channel
{
    internal sealed class ChannelConnectedState : ITeamCityChannelState
    {
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly IStateEngine<ITeamCityIoChannel> _stateEngine;
        private readonly IProjectRepository _projectRepository;
        private readonly IBuildAgentRepository _buildAgentRepository;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly ILog _logger;

        public ChannelConnectedState(ITcSharpTeamCityClient teamCityClient, IStateEngine<ITeamCityIoChannel> stateEngine, 
            IProjectRepository projectRepository, IBuildAgentRepository buildAgentRepository, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _teamCityClient = teamCityClient;
            _stateEngine = stateEngine;
            _projectRepository = projectRepository;
            _buildAgentRepository = buildAgentRepository;
            _channelStateBroadcaster = channelStateBroadcaster;
            _logger = LogManager.GetLogger("DateSources.TeamCity.Connected");
        }

        public string[] ProjectNames => _projectRepository.GetAll().Select(x => x.Name).ToArray();

        public bool IsConnected => true;

        public void Connect() {}

        public void Disconnect()
        {
            _stateEngine.OnDisconnected();
        }

        public string[] GetConfigurationNames(string projectName)
        {
            _logger.DebugFormat("Request for configuration names for project {0}", projectName);

            var configurations = _projectRepository.Get(projectName).Configurations;
            return configurations.Select(x => x.Name).ToArray();
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

        void ITeamCityChannelState.Leave() {}

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
    }
}