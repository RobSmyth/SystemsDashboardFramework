using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects
{
    public sealed class ProjectRepository : IProjectRepository
    {
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly IProjectFactory _projectFactory;
        private readonly ILog _logger;
        private readonly IDictionary<string, IProject> _projects = new Dictionary<string, IProject>();
        private readonly object _syncRoot = new object();
        private readonly IList<IDataChangeListener> _listeners = new List<IDataChangeListener>();

        public ProjectRepository(ITcSharpTeamCityClient teamCityClient, IProjectFactory projectFactory, IConnectedStateTicker ticker)
        {
            _teamCityClient = teamCityClient;
            _projectFactory = projectFactory;
            _logger = LogManager.GetLogger("Repositories.TeamCity.Projects");
            ticker.AddListener(this, Update);
        }

        public void AddListener(IDataChangeListener listener)
        {
            _listeners.Add(listener);
        }

        public IProject[] GetAll()
        {
            return _projects.Values.ToArray();
        }

        public void Add(IProject project)
        {
            lock (_syncRoot)
            {
                _projects.Add(project.Name, project);
                NotifyValueChanged();
            }
        }

        public IProject Add(string name)
        {
            lock (_syncRoot)
            {
                var project = new NullInteropProject(name);
                _projects.Add(name, _projectFactory.Create(project));
                NotifyValueChanged();
                return _projects[name];
            }
        }

        public IProject Get(string name)
        {
            lock (_syncRoot)
            {
                if (!_projects.ContainsKey(name))
                {
                    Add(name);
                }
                return _projects[name];
            }
        }

        private void Update()
        {
            var updated = new List<IProject>();
            List<TeamCitySharp.DomainEntities.Project> teamCityProjects;
            try
            {
                teamCityProjects = _teamCityClient.Projects.All();
            }
            catch (Exception)
            {
                teamCityProjects = new List<TeamCitySharp.DomainEntities.Project>();                
            }
            var changed = false;

            foreach (var teamCitySharpProject in teamCityProjects.Where(teamCityProject => !_projects.ContainsKey(teamCityProject.Name)))
            {
                _projects.Add(teamCitySharpProject.Name, _projectFactory.Create(teamCitySharpProject));
                changed = true;
            }

            foreach (var teamCityProject in teamCityProjects)
            {
                var project = _projects[teamCityProject.Name];
                project.Update(teamCityProject);
                updated.Add(project);
            }
            changed |= updated.Any();

            foreach (var orphanedProject in _projects.Values.ToArray().Except(updated))
            {
                orphanedProject.Update(new NullInteropProject(orphanedProject.Name));
            }

            if (changed)
            {
                NotifyValueChanged();       
            }
        }

        private void NotifyValueChanged()
        {
            var listeners = _listeners.ToArray();
            foreach (var listener in listeners)
            {
                listener.OnChanged();
            }
        }
    }
}
