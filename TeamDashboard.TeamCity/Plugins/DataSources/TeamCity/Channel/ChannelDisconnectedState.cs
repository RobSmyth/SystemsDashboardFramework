using System;
using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel
{
    internal sealed class ChannelDisconnectedState : ITeamCityChannelState
    {
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly ITeamCityDataSourceConfiguration _configuration;
        private readonly IBuildAgentRepository _buildAgentRepository;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly IStateEngine<ITeamCityIoChannel> _stateEngine;
        private readonly ILog _logger;
        private bool _testingConnection;

        public ChannelDisconnectedState(ITcSharpTeamCityClient teamCityClient, IStateEngine<ITeamCityIoChannel> stateEngine, ITeamCityDataSourceConfiguration configuration, IBuildAgentRepository buildAgentRepository, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _teamCityClient = teamCityClient;
            _stateEngine = stateEngine;
            _configuration = configuration;
            _buildAgentRepository = buildAgentRepository;
            _channelStateBroadcaster = channelStateBroadcaster;

            _logger = LogManager.GetLogger("DateSources.TeamCity.Disconnected");
        }

        public string[] ProjectNames => new string[0];
        public bool IsConnected => false;

        public void Connect()
        {
            if (_testingConnection)
            {
                return;
            }

            _testingConnection = true;

            _logger.Debug("Attempt to connect.");

            Task.Run(() => { _teamCityClient.Connect(_configuration.UserName, _configuration.Password); })
                .ContinueWith(x =>
                {
                    try
                    {
                        if (_teamCityClient.Authenticate())
                        {
                            _logger.Info("Has connected.");
                            _stateEngine.OnConnected();
                        }
                        else
                        {
                            _logger.Debug("Connection failed.");
                        }
                    }
                    catch (Exception exception)
                    {
                        _logger.Debug("Connection failed.", exception);
                    }
                    finally
                    {
                        _testingConnection = false;
                    }
                });
        }

        public void Disconnect()
        {
        }

        public IBuildAgent[] GetAgents()
        {
            return new IBuildAgent[0];
        }

        public Task<IBuildAgent> GetAgent(string name)
        {
            return Task.Run(() => _buildAgentRepository.Get(name));
        }

        void ITeamCityChannelState.Leave()
        {
        }

        void ITeamCityChannelState.Enter()
        {
            _channelStateBroadcaster.OnDisconnected.Fire();
        }
    }
}