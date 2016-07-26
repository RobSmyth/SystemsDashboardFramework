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
        private readonly IDataSource _dataSource;
        private readonly ITcSharpTeamCityClient _teamCitySharpClient;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly IBuildAgentViewModelFactory _buildAgentFactory;
        private readonly IDictionary<string, IBuildAgent> _buildAgents = new Dictionary<string, IBuildAgent>();
        private readonly object _syncRoot = new object();

        public BuildAgentRepository(IDataSource dataSource, ITcSharpTeamCityClient teamCitySharpClient, 
            IChannelConnectionStateBroadcaster channelStateBroadcaster, IConnectedStateTicker ticker, IBuildAgentViewModelFactory buildAgentFactory)
        {
            _dataSource = dataSource;
            _teamCitySharpClient = teamCitySharpClient;
            _channelStateBroadcaster = channelStateBroadcaster;
            _buildAgentFactory = buildAgentFactory;
            UpdateCounts();
            ticker.AddListener(OnTick);
        }

        public IBuildAgent[] GetAll()
        {
            return _buildAgents.Values.ToArray();
        }

        public void Add(IBuildAgent buildAgent)
        {
            _buildAgents.Add(buildAgent.Name.ToLower(), buildAgent);
            UpdateAgentData(buildAgent);
            UpdateCounts();
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
            var connectedAgents = _teamCitySharpClient.Agents.AllConnected();

            foreach (var agent in namedAgents)
            {
                var isAuthorised = authorisedAgents.Any(x => x.Name.Equals(agent.Name, StringComparison.CurrentCultureIgnoreCase));
                Get(agent.Name).IsAuthorised = isAuthorised;
            }

            foreach (var agent in namedAgents)
            {
                var buildAgent = Get(agent.Name);
                var isOnline = connectedAgents.Any(x => x.Name.Equals(agent.Name, StringComparison.CurrentCultureIgnoreCase));
                buildAgent.IsOnline = isOnline;
            }

            foreach (var buildAgent in GetAll())
            {
                UpdateAgentData(buildAgent);
            }

            UpdateCounts();
        }

        private void OnTick()
        {
            Update();
        }

        private void UpdateAgentData(IBuildAgent buildAgent)
        {
            _dataSource.Write($"Agent({buildAgent.Name})", "");
            _dataSource.Write($"Agent({buildAgent.Name}).IsAuthorised", buildAgent.IsAuthorised);
            _dataSource.Write($"Agent({buildAgent.Name}).IsOnline", buildAgent.IsOnline);
            _dataSource.Write($"Agent({buildAgent.Name}).IsRunning", buildAgent.IsRunning);
        }

        private void UpdateCounts()
        {
            var buildAgents = GetAll();
            _dataSource.Write($"Count", buildAgents.Length);
            _dataSource.Write($"Count.Authorised", buildAgents.Count(x => x.IsAuthorised));
            _dataSource.Write($"Count.Online", buildAgents.Count(x => x.IsOnline));
            _dataSource.Write($"Count.Running", buildAgents.Count(x => x.IsRunning));
        }
    }
}