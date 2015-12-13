using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    public class BuildAgentRepository : IBuildAgentRepository
    {
        private readonly IDictionary<string, IBuildAgent> _buildAgents = new Dictionary<string, IBuildAgent>();

        public IBuildAgent[] GetAll()
        {
            return _buildAgents.Values.ToArray();
        }

        public void Add(IBuildAgent buildAgent)
        {
            _buildAgents.Add(buildAgent.Name.ToLower(), buildAgent);
        }

        public IBuildAgent Get(string name)
        {
            var normalisedName = name.ToLower();
            if (!_buildAgents.ContainsKey(normalisedName))
            {
                return new NullBuildAgent(name);
            }
            return _buildAgents[normalisedName];
        }

        public bool Has(string name)
        {
            return _buildAgents.ContainsKey(name.ToLower());
        }
    }
}
