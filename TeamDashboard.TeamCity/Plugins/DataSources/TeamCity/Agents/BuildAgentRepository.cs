using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents
{
    public sealed class BuildAgentRepository : IBuildAgentRepository
    {
        private readonly IDataSource _outerRepository;
        private readonly ITcSharpTeamCityClient _teamCitySharpClient;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly IBuildAgentViewModelFactory _buildAgentFactory;
        private readonly IDictionary<string, IBuildAgent> _buildAgents = new Dictionary<string, IBuildAgent>();
        private readonly object _syncRoot = new object();

        public BuildAgentRepository(IDataSource outerRepository, ITcSharpTeamCityClient teamCitySharpClient, 
            IChannelConnectionStateBroadcaster channelStateBroadcaster, IConnectedStateTicker ticker, IBuildAgentViewModelFactory buildAgentFactory)
        {
            _outerRepository = outerRepository;
            _teamCitySharpClient = teamCitySharpClient;
            _channelStateBroadcaster = channelStateBroadcaster;
            _buildAgentFactory = buildAgentFactory;
            _outerRepository.Write($"Agents.Count", 0);
            ticker.AddListener(OnTick);
        }

        public IBuildAgent[] GetAll()
        {
            return _buildAgents.Values.ToArray();
        }

        public void Add(IBuildAgent buildAgent)
        {
            _buildAgents.Add(buildAgent.Name.ToLower(), buildAgent);
            _outerRepository.Write($"Agents.Count", _buildAgents.Count);
        }

        public IBuildAgent Get(string name)
        {
            var normalisedName = name.ToLower();
            lock (_syncRoot)
            {
                if (!Has(normalisedName))
                {
                    var buildAgent = _buildAgentFactory.Create(name, _channelStateBroadcaster);
                    Add(buildAgent);
                    return buildAgent;
                }
                return _buildAgents[normalisedName];
            }
        }

        public bool Has(string name)
        {
            return _buildAgents.ContainsKey(name.ToLower());
        }

        private void Update()
        {
            var namedAgents = _buildAgents.Values.Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToArray();

            var authorisedAgents = _teamCitySharpClient.Agents.AllAuthorised();
            foreach (var agent in namedAgents)
            {
                var isAuthorised = authorisedAgents.Any(x => x.Name.Equals(agent.Name, StringComparison.CurrentCultureIgnoreCase));
                Get(agent.Name).IsAuthorised = isAuthorised;
            }

            var connectedAgents = _teamCitySharpClient.Agents.AllConnected();
            foreach (var agent in namedAgents)
            {
                var buildAgent = Get(agent.Name);
                var isOnline = connectedAgents.Any(x => x.Name.Equals(agent.Name, StringComparison.CurrentCultureIgnoreCase));
                buildAgent.IsOnline = isOnline;
            }
        }

        private void OnTick()
        {
            Update();
        }
    }
}