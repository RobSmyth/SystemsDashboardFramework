using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents
{
    public sealed class BuildAgentRepository : IBuildAgentRepository, ITimerListener
    {
        private readonly IDataSource _outerRepository;
        private readonly TcSharpTeamCityClient _teamCityClient;
        private readonly IServices _services;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly IDictionary<string, IBuildAgent> _buildAgents = new Dictionary<string, IBuildAgent>();
        private Action _onDisconnected = () => { };
        private Action _onConnected = () => { };
        private ITimerToken _timerToken;

        public BuildAgentRepository(IDataSource outerRepository, TcSharpTeamCityClient teamCityClient, IServices services, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _outerRepository = outerRepository;
            _teamCityClient = teamCityClient;
            _services = services;
            _channelStateBroadcaster = channelStateBroadcaster;
            _channelStateBroadcaster.OnConnected.AddListener(OnConnected);
            _channelStateBroadcaster.OnDisconnected.AddListener(OnDisconnected);
            _outerRepository.Write($"Agents.Count", 0);
            SetDisconnectedState();
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
            if (!Has(normalisedName))
            {
                var buildAgent = new TeamCityBuildAgentViewModel(name, _services.Timer, _outerRepository, _channelStateBroadcaster);
                Add(buildAgent);
                return buildAgent;
            }

            return _buildAgents[normalisedName];
        }

        public bool Has(string name)
        {
            return _buildAgents.ContainsKey(name.ToLower());
        }

        private void Update()
        {
            var currentAgents = _teamCityClient.Agents.All();
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

        private void OnDisconnected()
        {
            _onDisconnected();
        }

        private void OnConnected()
        {
            var action = _onConnected;
            SetConnectedState();
            action();
        }

        public void OnTimeElapsed(TimerToken token)
        {
            Update();
            _timerToken = _services.Timer.QueueCallback(TimeSpan.FromMinutes(5), this);
        }

        private void SetDisconnectedState()
        {
            _onDisconnected = () => { };
            _onConnected = () => { _timerToken = _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this); };
        }

        private void SetConnectedState()
        {
            _onDisconnected = () => { SetDisconnectedState(); };
            _onConnected = () => { };
        }
    }
}