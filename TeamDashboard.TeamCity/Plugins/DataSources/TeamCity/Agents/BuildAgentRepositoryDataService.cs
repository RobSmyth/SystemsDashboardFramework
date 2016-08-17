using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents
{
    public sealed class BuildAgentRepositoryDataService : IService, IDataChangeListener
    {
        private readonly IBuildAgentRepository _repository;
        private readonly IDataSource _dataSource;

        public BuildAgentRepositoryDataService(IBuildAgentRepository repository, IDataSource dataSource)
        {
            _repository = repository;
            _dataSource = dataSource;
        }

        public string Name => "TeamCity.Agents";

        public void Stop()
        {
        }

        public void Start()
        {
            UpdateBuildAgents();
            _repository.AddListener(this);
        }

        private void UpdateBuildAgents()
        {
            var BuildAgents = _repository.GetAll();
            _dataSource.Write($"Count", BuildAgents.Length);
            foreach (var buildAgent in BuildAgents)
            {
                if (string.IsNullOrWhiteSpace(buildAgent.Name))
                {
                    continue;
                }

                _dataSource.Write($"BuildAgent({buildAgent.Name})", "-");
            }
        }

        void IDataChangeListener.OnChanged()
        {
            UpdateBuildAgents();
        }
    }
}