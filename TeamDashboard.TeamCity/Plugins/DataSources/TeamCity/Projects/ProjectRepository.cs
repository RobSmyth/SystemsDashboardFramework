using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects
{
    public sealed class ProjectRepository : IProjectRepository
    {
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly ProjectFactory _projectFactory;
        private readonly ILog _logger;
        private readonly IDictionary<string, IProject> _projects = new Dictionary<string, IProject>();
        private readonly object _syncRoot = new object();

        public ProjectRepository(IDataSource outerRepository, ITcSharpTeamCityClient teamCityClient, ProjectFactory projectFactory, IConnectedStateTicker connectedTicker)
        {
            _teamCityClient = teamCityClient;
            _projectFactory = projectFactory;
            outerRepository.Write($"Agents.Count", 0);
            _logger = LogManager.GetLogger("Repositories.Projects");
            connectedTicker.AddListener(Update);
        }

        public IProject[] GetAll()
        {
            return _projects.Values.ToArray();
        }

        public IProject Get(string name)
        {
            lock (_syncRoot)
            {
                if (!_projects.ContainsKey(name.ToLower()))
                {
                    _projects.Add(name.ToLower(), _projectFactory.Create(new NullInteropProject(name)));
                }
                return _projects[name.ToLower()];
            }
        }

        private void Update()
        {
            var updated = new List<IProject>();
            var teamCityProjects = _teamCityClient.Projects.All();

            foreach (var teamCitySharpProject in teamCityProjects.Where(teamCityProject => !_projects.ContainsKey(teamCityProject.Name.ToLower())))
            {
                _projects.Add(teamCitySharpProject.Name.ToLower(), _projectFactory.Create(teamCitySharpProject));
            }

            foreach (var teamCityProject in teamCityProjects)
            {
                var project = _projects[teamCityProject.Name.ToLower()];
                project.Update(teamCityProject);
                updated.Add(project);
            }

            foreach (var orphanedProject in _projects.Values.ToArray().Except(updated))
            {
                orphanedProject.Update(new NullInteropProject(orphanedProject.Name));
            }
        }
    }
}
