using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects
{
    public sealed class ProjectRepositoryDataService : IProjectRepositoryDataService, IDataChangeListener
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDataSource _dataSource;

        public ProjectRepositoryDataService(IProjectRepository projectRepository, IDataSource dataSource)
        {
            _projectRepository = projectRepository;
            _dataSource = dataSource;
        }

        public string Name => "TeamCity.Projects";

        public void Stop()
        {
        }

        public void Start()
        {
            UpdateProjects();
            _projectRepository.AddListener(this);
        }

        private void UpdateProjects()
        {
            var projects = _projectRepository.GetAll();
            _dataSource.Write($"Count", projects.Length);
            foreach (var project in projects)
            {
                _dataSource.Write("Project." + project.Name, "-");
            }
        }

        void IDataChangeListener.OnChanged()
        {
            UpdateProjects();
        }
    }
}