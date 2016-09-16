using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects
{
    public sealed class ProjectRepositoryDataService : IProjectRepositoryDataService, IDataChangeListener
    {
        private readonly IProjectRepository _repository;
        private readonly IDataSource _dataSource;

        public ProjectRepositoryDataService(IDataSource dataSource, IProjectRepository repository)
        {
            _repository = repository;
            _dataSource = dataSource;
        }

        public string Name => "TeamCity.Projects";

        public void Stop()
        {
        }

        public void Start()
        {
            UpdateProjects();
            _repository.AddListener(this);
        }

        private void UpdateProjects()
        {
            var projects = _repository.GetAll();
            _dataSource.Write($"Count", projects.Length);
            foreach (var project in projects)
            {
                _dataSource.Write($"Project.{project.Name}", "-");
                foreach (var buildConfiguration in project.Configurations)
                {
                    _dataSource.Write($"Project.{project.Name}.Configuration.{buildConfiguration.Name}", "-");
                }
            }
        }

        void IDataChangeListener.OnChanged()
        {
            UpdateProjects();
        }
    }
}