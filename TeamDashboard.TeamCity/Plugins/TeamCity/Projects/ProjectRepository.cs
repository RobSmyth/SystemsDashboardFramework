using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoeticTools.TeamStatusBoard.Framework.DataSources.Jira;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDataSource _outerRepository;
        private readonly ITimeCachedArray<Project> _projectCache;
        private readonly IDictionary<string, IProject> _projects = new Dictionary<string, IProject>();

        public ProjectRepository(IDataSource outerRepository, ITimeCachedArray<Project> projectCache)
        {
            _outerRepository = outerRepository;
            _projectCache = projectCache;
            _outerRepository.Write($"Projects.Count", 0);

            foreach (var project in projectCache.Items)
            {
                Add(project);
            }
        }

        public IProject[] GetAll()
        {
            return _projects.Values.ToArray();
        }

        public void Add(Project project)
        {
            _projects.Add(project.Name.ToLower(), new TeamCityProjectViewModel(project.Name, _projectCache));
            _outerRepository.Write($"Projects.Count", _projects.Count);
        }

        public IProject Get(string name)
        {
            var normalisedName = name.ToLower();
            if (!_projects.ContainsKey(normalisedName))
            {
                var nullAgent = new NullProject(name);
                return nullAgent;
            }
            return _projects[normalisedName];
        }

        public bool Has(string namet)
        {
            return _projects.ContainsKey(namet.ToLower());
        }
    }
}
