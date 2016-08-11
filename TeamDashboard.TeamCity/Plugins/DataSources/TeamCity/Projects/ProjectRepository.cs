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
        private readonly IList<IDataChangeListener> _listeners = new List<IDataChangeListener>();

        public ProjectRepository(ITcSharpTeamCityClient teamCityClient, ProjectFactory projectFactory, IConnectedStateTicker connectedTicker)
        {
            _teamCityClient = teamCityClient;
            _projectFactory = projectFactory;
            _logger = LogManager.GetLogger("Repositories.TeamCity.Projects");
            connectedTicker.AddListener(Update);
        }

        public void AddListener(IDataChangeListener listener)
        {
            _listeners.Add(listener);
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
                    NotifyValueChanged();
                }
                return _projects[name.ToLower()];
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

            foreach (var teamCitySharpProject in teamCityProjects.Where(teamCityProject => !_projects.ContainsKey(teamCityProject.Name.ToLower())))
            {
                _projects.Add(teamCitySharpProject.Name.ToLower(), _projectFactory.Create(teamCitySharpProject));
                changed = true;
            }

            foreach (var teamCityProject in teamCityProjects)
            {
                var project = _projects[teamCityProject.Name.ToLower()];
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
