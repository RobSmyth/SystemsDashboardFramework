using System.Linq;
using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel
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

        public Task<IBuildAgent> GetAgent(string name)
        {
            return Task.Run(() => _buildAgentRepository.Get(name));
        }

        public IBuildAgent[] GetAgents()
        {
            UpdateBuildAgentRepository();
            var agents = _buildAgentRepository.GetAll();

            return agents;
        }

        void ITeamCityChannelState.Leave() {}

        void ITeamCityChannelState.Enter()
        {
            Task.Run(() => GetAgents());
            _channelStateBroadcaster.OnConnected.Fire();
        }

        private void UpdateBuildAgentRepository()
        {
        }
    }
}