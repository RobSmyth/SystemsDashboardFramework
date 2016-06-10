using System.Collections.Generic;
using System.Linq;
using NoeticTool.sTeamStatusBoard.TeamCity.Plugins.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents
{
    public sealed class BuildAgentRepository : IBuildAgentRepository
    {
        private readonly IDataSource _outerRepository;
        private readonly IDictionary<string, IBuildAgent> _buildAgents = new Dictionary<string, IBuildAgent>();

        public BuildAgentRepository(IDataSource outerRepository)
        {
            _outerRepository = outerRepository;
            _outerRepository.Write($"Agents.Count", 0);
        }

        public IBuildAgent[] GetAll()
        {
            return _buildAgents.Values.ToArray();
        }

        public void Add(IBuildAgent buildAgent)
        {
            _buildAgents.Add(buildAgent.Name.ToLower(), buildAgent);
            UpdateBuildAgentParameters(buildAgent);
            _outerRepository.Write($"Agents.Count", _buildAgents.Count);
        }

        public IBuildAgent Get(string name)
        {
            var normalisedName = name.ToLower();
            if (!_buildAgents.ContainsKey(normalisedName))
            {
                var nullAgent = new NullBuildAgent(name);
                UpdateBuildAgentParameters(nullAgent);
                return nullAgent;
            }
            var agent = _buildAgents[normalisedName];
            UpdateBuildAgentParameters(agent);
            return agent;
        }

        public bool Has(string name)
        {
            return _buildAgents.ContainsKey(name.ToLower());
        }

        private void UpdateBuildAgentParameters(IBuildAgent buildAgent)
        {
            _outerRepository.Write($"Agent.{buildAgent.Name}.Status", buildAgent.Status);
        }
    }
}