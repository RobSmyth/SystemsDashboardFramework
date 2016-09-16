﻿using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents
{
    public sealed class BuildAgentRepository : IBuildAgentRepository
    {
        private readonly IDataSource _dataSource;
        private readonly ITcSharpTeamCityClient _teamCitySharpClient;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly IBuildAgentViewModelFactory _buildAgentFactory;
        private readonly IDictionary<string, IBuildAgent> _buildAgents = new Dictionary<string, IBuildAgent>();
        private readonly object _syncRoot = new object();
        private readonly IList<IDataChangeListener> _listeners = new List<IDataChangeListener>();

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

        public void AddListener(IDataChangeListener listener)
        {
            _listeners.Add(listener);
            listener.OnChanged();
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

            NotifyValueChanged(); // todo
        }

        private void OnTick()
        {
            Update();
        }

        private void UpdateAgentData(IBuildAgent buildAgent)
        {
            if (string.IsNullOrWhiteSpace(buildAgent.Name))
            {
                return;
            }

            _dataSource.Write($"Agent.{buildAgent.Name}", "");
            _dataSource.Write($"Agent.{buildAgent.Name}.IsAuthorised", buildAgent.IsAuthorised);
            _dataSource.Write($"Agent.{buildAgent.Name}.IsOnline", buildAgent.IsOnline);
            _dataSource.Write($"Agent.{buildAgent.Name}.IsRunning", buildAgent.IsRunning);
        }

        private void UpdateCounts()
        {
            var buildAgents = GetAll();
            _dataSource.Write($"Count", buildAgents.Length);
            _dataSource.Write($"Count.Authorised", buildAgents.Count(x => x.IsAuthorised));
            _dataSource.Write($"Count.Online", buildAgents.Count(x => x.IsOnline));
            _dataSource.Write($"Count.Running", buildAgents.Count(x => x.IsRunning));
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