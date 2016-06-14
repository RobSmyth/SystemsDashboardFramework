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
    public sealed class BuildAgentRepository : IBuildAgentRepository, ITimerListener, IChannelConnectionStateListener
    {
        private readonly TimeSpan _updateDelayOnConnection = TimeSpan.FromMilliseconds(100);
        private readonly TimeSpan _updatePeriod = TimeSpan.FromMinutes(5);
        private readonly IDataSource _outerRepository;
        private readonly ITcSharpTeamCityClient _teamCitySharpClient;
        private readonly IServices _services;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly IDictionary<string, IBuildAgent> _buildAgents = new Dictionary<string, IBuildAgent>();
        private Action _onDisconnected = () => { };
        private Action _onConnected = () => { };
        private ITimerToken _timerToken = new NullTimerToken();
        private readonly object _syncRoot = new object();

        public BuildAgentRepository(IDataSource outerRepository, ITcSharpTeamCityClient teamCitySharpClient, IServices services, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _outerRepository = outerRepository;
            _teamCitySharpClient = teamCitySharpClient;
            _services = services;
            _channelStateBroadcaster = channelStateBroadcaster;
            _channelStateBroadcaster.Add(this);
            _outerRepository.Write($"Agents.Count", 0);
            EnterDisconnectedState();
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
                    var buildAgent = new TeamCityBuildAgentViewModel(name, _services.Timer, _outerRepository, _channelStateBroadcaster, _teamCitySharpClient);
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
            var currentAgents = _teamCitySharpClient.Agents.All();
            foreach (var teamCityAgent in currentAgents)
            {
                Get(teamCityAgent.Name);
            }

            var orphanedagents = _buildAgents.Values.Where(x => !currentAgents.Any(y => y.Name.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase)));
            foreach (var orphanedagent in orphanedagents)
            {
                orphanedagent.IsNotKnown();
            }
        }

        // todo - duplication, see Project.cs
        void IChannelConnectionStateListener.OnDisconnected()
        {
            var action = _onDisconnected;
            EnterDisconnectedState();
            action();
        }

        // todo - duplication, see Project.cs
        void IChannelConnectionStateListener.OnConnected()
        {
            var action = _onConnected;
            EnterConnectedState();
            action();
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _timerToken = _services.Timer.QueueCallback(_updatePeriod, this);
        }

        private void EnterDisconnectedState()
        {
            _onDisconnected = () => { };
            _onConnected = () =>
            {
                _timerToken.Cancel();
                _timerToken = _services.Timer.QueueCallback(_updateDelayOnConnection, this);
            };
        }

        // todo - duplication with ProjectRepository
        private void EnterConnectedState()
        {
            _onDisconnected = () =>
            {
                var token = _timerToken;
                _timerToken = new NullTimerToken();
                token.Cancel();
            };
            _onConnected = () => { };
        }
    }
}