using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents
{
    public sealed class BuildAgentRepository : IBuildAgentRepository
    {
        private readonly IDataSource _outerRepository;
        private readonly TcSharpTeamCityClient _teamCityClient;
        private readonly IServices _services;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly IDictionary<string, IBuildAgent> _buildAgents = new Dictionary<string, IBuildAgent>();

        public BuildAgentRepository(IDataSource outerRepository, TcSharpTeamCityClient teamCityClient, IServices services, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _outerRepository = outerRepository;
            _teamCityClient = teamCityClient;
            _services = services;
            _channelStateBroadcaster = channelStateBroadcaster;
            _outerRepository.Write($"Agents.Count", 0);
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
                var buildAgent = new TeamCityBuildAgentViewModel(name, _teamCityClient, _services.Timer, _outerRepository, _channelStateBroadcaster);
                Add(buildAgent);
                return buildAgent;
            }

            return _buildAgents[normalisedName];
        }

        public bool Has(string name)
        {
            return _buildAgents.ContainsKey(name.ToLower());
        }

        public void StopWatching()
        {
            foreach (var buildAgent in _buildAgents)
            {
            }
        }
    }
}