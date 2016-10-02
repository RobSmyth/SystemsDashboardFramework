using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents
{
    public sealed class BuildAgentRepository : IBuildAgentRepository
    {
        private const string PropertyTag = "TeamCity.BuildAgent";
        private readonly IDataSource _dataSource;
        private readonly ITcSharpTeamCityClient _teamCitySharpClient;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly IBuildAgentViewModelFactory _buildAgentFactory;
        private readonly ITeamCityDataSourceConfiguration _teamCityDataSourceConfiguration;
        private readonly IDictionary<string, IBuildAgent> _buildAgents = new Dictionary<string, IBuildAgent>();
        private readonly object _syncRoot = new object();
        private readonly IList<IDataChangeListener> _listeners = new List<IDataChangeListener>();

        public BuildAgentRepository(IDataSource dataSource, ITcSharpTeamCityClient teamCitySharpClient, 
            IChannelConnectionStateBroadcaster channelStateBroadcaster, IConnectedStateTicker ticker, IBuildAgentViewModelFactory buildAgentFactory,
            ITeamCityDataSourceConfiguration teamCityDataSourceConfiguration)
        {
            _dataSource = dataSource;
            _teamCitySharpClient = teamCitySharpClient;
            _channelStateBroadcaster = channelStateBroadcaster;
            _buildAgentFactory = buildAgentFactory;
            _teamCityDataSourceConfiguration = teamCityDataSourceConfiguration;
            UpdateCounts();
            ticker.AddListener(this, OnTick);
        }

        public IBuildAgent[] GetAll()
        {
            return _buildAgents.Values.OrderBy(x => x.Name).ToArray();
        }

        public void Add(IBuildAgent buildAgent)
        {
            _buildAgents.Add(buildAgent.Name, buildAgent);
            UpdateAgentData(buildAgent);
            UpdateCounts();
        }

        public IBuildAgent Add(string name)
        {
            var buildAgent = _buildAgentFactory.Create(name, _channelStateBroadcaster);
            Add(buildAgent);
            return buildAgent;
        }

        public IBuildAgent Get(string name)
        {
            lock (_syncRoot)
            {
                var shortName = GetShortName(name);
                if (!Has(shortName))
                {
                    Add(shortName);
                }
                return _buildAgents[shortName];
            }
        }

        public bool Has(string name)
        {
            return _buildAgents.ContainsKey(GetShortName(name));
        }

        public void AddListener(IDataChangeListener listener)
        {
            _listeners.Add(listener);
            listener.OnChanged();
        }

        private void Update()
        {
            var namedAgents = _buildAgents.Values.Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToArray();
            var authorisedAgents = _teamCitySharpClient.Agents.AllAuthorised().Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToArray();
            var connectedAgents = _teamCitySharpClient.Agents.AllConnected().Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToArray();

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
            NotifyValueChanged();
        }

        private string GetShortName(string name)
        {
            var longNamePrefix = $"{_dataSource.TypeName}.Agents.";
            if (name.StartsWith(longNamePrefix))
            {
                return name.Substring(longNamePrefix.Length);
            }
            return name;
        }

        private bool IsFiltered(string name)
        {
            var regex = new Regex(_teamCityDataSourceConfiguration.AgentsFilter, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(name))
            {
                return true;
            }
            return false;
        }

        private void OnTick()
        {
            Update();
        }

        private void UpdateAgentData(IBuildAgent buildAgent)
        {
            if (string.IsNullOrWhiteSpace(buildAgent.Name) || IsFiltered(buildAgent.Name))
            {
                return;
            }

            buildAgent.UpdateProperties();
        }

        private void UpdateCounts()
        {
            var buildAgents = GetAll().Where(x => !IsFiltered(x.Name)).ToArray();

            _dataSource.Set($"Agents.Count", buildAgents.Length, PropertiesFlags.ReadOnly, PropertyTag);
            _dataSource.Set($"Agents.Count.Authorised", buildAgents.Count(x => x.IsAuthorised), PropertiesFlags.ReadOnly, PropertyTag);
            _dataSource.Set($"Agents.Count.Online", buildAgents.Count(x => x.IsOnline), PropertiesFlags.ReadOnly, PropertyTag);
            _dataSource.Set($"Agents.Count.Running", buildAgents.Count(x => x.IsRunning), PropertiesFlags.ReadOnly, PropertyTag);

            NotifyValueChanged();
        }

        private void NotifyValueChanged()
        {
            var listeners = _listeners.ToArray();
            foreach (var listener in listeners)
            {
                listener.OnChanged();
            }
        }
    }
}