using System;
using System.Linq;
using System.Text.RegularExpressions;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TcSharpInterop;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Agents
{
    public sealed class BuildAgentsStatusService : IDataChangeListener
    {
        private readonly IBuildAgentRepository _repository;
        private readonly ITcSharpTeamCityClient _tcsClient;
        private readonly IDataSource _dataSource;
        private readonly ITeamCityDataSourceConfiguration _configuration;
        private const string PropertyTag = "TeamCity.BuildAgent";

        public BuildAgentsStatusService(IBuildAgentRepository repository, ITcSharpTeamCityClient tcsClient, IDataSource dataSource, IConnectedStateTicker ticker, ITeamCityDataSourceConfiguration configuration)
        {
            _repository = repository;
            _tcsClient = tcsClient;
            _dataSource = dataSource;
            _configuration = configuration;
            UpdateCounts();
            ticker.AddListener(this, OnTick);
        }

        private void OnTick()
        {
            var namedAgents = _repository.GetAll().Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToArray();
            var authorisedAgents = _tcsClient.Agents.AllAuthorised().Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToArray();
            var connectedAgents = _tcsClient.Agents.AllConnected().Where(x => !string.IsNullOrWhiteSpace(x.Name)).ToArray();

            foreach (var agent in namedAgents)
            {
                var isAuthorised = authorisedAgents.Any(x => x.Name.Equals(agent.Name, StringComparison.CurrentCultureIgnoreCase));
                _repository.Get(agent.Name).IsAuthorised = isAuthorised;
            }

            foreach (var agent in namedAgents)
            {
                var buildAgent = _repository.Get(agent.Name);
                var isOnline = connectedAgents.Any(x => x.Name.Equals(agent.Name, StringComparison.CurrentCultureIgnoreCase));
                buildAgent.IsOnline = isOnline;
            }

            UpdateAgentData();
        }

        private void UpdateCounts()
        {
            var buildAgents = _repository.GetAll().Where(x => !IsFiltered(x.Name)).ToArray();

            _dataSource.Set($"Agents.Count", buildAgents.Length, PropertiesFlags.ReadOnly, PropertyTag);
            _dataSource.Set($"Agents.Count.Authorised", buildAgents.Count(x => x.IsAuthorised), PropertiesFlags.ReadOnly, PropertyTag);
            _dataSource.Set($"Agents.Count.Online", buildAgents.Count(x => x.IsOnline), PropertiesFlags.ReadOnly, PropertyTag);
            _dataSource.Set($"Agents.Count.Running", buildAgents.Count(x => x.IsRunning), PropertiesFlags.ReadOnly, PropertyTag);
        }

        private bool IsFiltered(string name)
        {
            var regex = new Regex(_configuration.AgentsFilter, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(name))
            {
                return true;
            }
            return false;
        }

        void IDataChangeListener.OnChanged()
        {
            UpdateCounts();
            UpdateAgentData();
        }

        private void UpdateAgentData()
        {
            foreach (var buildAgent in _repository.GetAll())
            {
                buildAgent.UpdateProperties();
                if (!string.IsNullOrWhiteSpace(buildAgent.Name) && !IsFiltered(buildAgent.Name))
                {
                    buildAgent.UpdateProperties();
                }

            }
        }
    }
}