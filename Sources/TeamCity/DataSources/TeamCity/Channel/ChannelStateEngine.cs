using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TcSharpInterop;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel
{
    public class ChannelStateEngine : IStateEngine<ITeamCityIoChannel>
    {
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly IProjectRepository _projectRepository;
        private readonly IBuildAgentRepository _buildAgentRepository;
        private readonly IDataSource _repository;
        private readonly ITeamCityDataSourceConfiguration _configuration;
        private readonly ChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly object _syncRoot = new object();

        public ChannelStateEngine(IServices services, ITcSharpTeamCityClient teamCityClient, IProjectRepository projectRepository, IBuildAgentRepository buildAgentRepository, IDataSource repository, ITeamCityDataSourceConfiguration configuration, ChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _teamCityClient = teamCityClient;
            _projectRepository = projectRepository;
            _buildAgentRepository = buildAgentRepository;
            _repository = repository;
            _configuration = configuration;
            _channelStateBroadcaster = channelStateBroadcaster;
            Current = services.RunOptions.EmulateMode ? new ChannelEmulatedState(repository) : CreateDisconnectedState();
        }

        public ITeamCityIoChannel Current { get; private set; }

        public void OnConnected()
        {
            _repository.Write("Service.Status", "Connected");
            _repository.Write("Service.Connected", true);
            ChangeState(new ChannelConnectedState(_teamCityClient, this, _projectRepository, _buildAgentRepository, _channelStateBroadcaster));
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
            ChangeState(new ChannelStoppedState());
        }

        public void Start()
        {
            Current = CreateDisconnectedState();
        }

        private ITeamCityChannelState CreateDisconnectedState()
        {
            return new ChannelDisconnectedState(_teamCityClient, this, _configuration, _buildAgentRepository, _channelStateBroadcaster);
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