using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects
{
    public sealed class ProjectRepositoryDataService : IProjectRepositoryDataService, IDataChangeListener
    {
        private const string PropertyTag = "TeamCity.Project";
        private readonly IProjectRepository _repository;
        private readonly IDataSource _dataSource;

        public ProjectRepositoryDataService(IDataSource dataSource, IProjectRepository repository, IConnectedStateTicker ticker)
        {
            _repository = repository;
            _dataSource = dataSource;
            ticker.AddListener(this, UpdateProjects);
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
            foreach (var project in projects)
            {
                if (project.Archived || project.Name.Contains("Archived"))
                {
                    continue;
                }

                _dataSource.Set($"Projects.{project.Name}", project, PropertiesFlags.ReadOnly, PropertyTag, "Ref");
                _dataSource.Set($"Projects.{project.Name}.Id", project.Id, PropertiesFlags.ReadOnly, PropertyTag);
                _dataSource.Set($"Projects.{project.Name}.Url", project.WebUrl, PropertiesFlags.ReadOnly, PropertyTag);

                foreach (var buildConfiguration in project.Configurations)
                {
                    if (buildConfiguration.Name.Contains("Archived"))
                    {
                        continue;
                    }
                    _dataSource.Set($"Projects.{project.Name}.Configuration.{buildConfiguration.Name}", buildConfiguration, PropertiesFlags.ReadOnly, "TeamCity.BuildConfiguration", "Ref");
                }
            }
        }

        void IDataChangeListener.OnChanged()
        {
            UpdateProjects();
        }
    }
}