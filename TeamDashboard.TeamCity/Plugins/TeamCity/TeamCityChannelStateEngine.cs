using System;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public class TeamCityChannelStateEngine : IStateEngine<ITeamCityChannel>
    {
        private readonly IServices _services;
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly IProjectRepository _projectRepository;
        private readonly IBuildAgentRepository _buildAgentRepository;
        private readonly IDataSource _repository;
        private readonly ITeamCityServiceConfiguration _configuration;
        private readonly ChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly object _syncRoot = new object();

        public TeamCityChannelStateEngine(IServices services, ITcSharpTeamCityClient teamCityClient, IProjectRepository projectRepository, IBuildAgentRepository buildAgentRepository, IDataSource repository, ITeamCityServiceConfiguration configuration, ChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _services = services;
            _teamCityClient = teamCityClient;
            _projectRepository = projectRepository;
            _buildAgentRepository = buildAgentRepository;
            _repository = repository;
            _configuration = configuration;
            _channelStateBroadcaster = channelStateBroadcaster;
            Current = services.RunOptions.EmulateMode ? new TeamCityChannelEmulatedState(repository) : CreateDisconnectedState();
        }

        public ITeamCityChannel Current { get; private set; }

        public void OnConnected()
        {
            _repository.Write("Service.Status", "Connected");
            _repository.Write("Service.Connected", true);
            ChangeState(new TeamCityChannelConnectedState(_teamCityClient, this, _projectRepository, _buildAgentRepository, _services, _channelStateBroadcaster));
        }

        public void OnDisconnected()
        {
            _repository.Write("Service.Status", "Disconnected");
            _repository.Write("Service.Connected", false);
            ChangeState(CreateDisconnectedState());
        }

        public void Stop()
        {
            _repository.Write("Service.Status", "Stopped");
            _repository.Write("Service.Connected", false);
            ChangeState(new TeamCityChannelStoppedState());
        }

        public void Start()
        {
            Current = CreateDisconnectedState();
        }

        private ITeamCityChannelState CreateDisconnectedState()
        {
            return new TeamCityChannelDisconnectedState(_teamCityClient, this, _configuration, _buildAgentRepository, _services, _channelStateBroadcaster);
        }

        private void ChangeState(ITeamCityChannelState nextState)
        {
            lock (_syncRoot)
            {
                var priorState = (ITeamCityChannelState) Current;
                Current = nextState;
                priorState.Leave();
                nextState.Enter();
            }
        }
    }
}