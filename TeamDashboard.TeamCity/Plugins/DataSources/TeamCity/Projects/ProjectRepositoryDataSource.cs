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
            _dataSource.Write($"Count", 0);
            UpdateProjectNames();
            _projectRepository.AddListener(this);
        }

        private void UpdateProjectNames() // todo - make this on change in repository (needs change notification)
        {
            foreach (var projectName in _projectRepository.GetAll().Select(x => x.Name).ToArray())
            {
                _dataSource.Write("Project." + projectName, 0);
            }
        }

        void IDataChangeListener.OnChanged()
        {
            UpdateProjectNames();
        }
    }
}