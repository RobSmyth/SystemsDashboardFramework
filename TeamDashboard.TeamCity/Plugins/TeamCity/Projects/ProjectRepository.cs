using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.DataSources.Jira;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public sealed class ProjectRepository : IChannelConnectionStateListener, ITimerListener, IProjectRepository
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromMinutes(1);
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly IServices _services;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private Action _onConnected = () => { };
        private Action _onDisconnected = () => { };
        private ITimerToken _timerToken = new NullTimerToken();
        private readonly ILog _logger;
        private readonly IDictionary<string, IProject> _projects = new Dictionary<string, IProject>();

        public ProjectRepository(IDataSource outerRepository, ITcSharpTeamCityClient teamCityClient, IServices services, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _teamCityClient = teamCityClient;
            _services = services;
            _channelStateBroadcaster = channelStateBroadcaster;
            _channelStateBroadcaster.Add(this);
            outerRepository.Write($"Agents.Count", 0);
            _logger = LogManager.GetLogger("Repositories.Projects");
            SetDisconnectedState();
        }

        public IProject[] GetAll()
        {
            return _projects.Values.ToArray();
        }

        public IProject Get(string name)
        {
            if (!_projects.ContainsKey(name.ToLower()))
            {
                _projects.Add(name.ToLower(), new Project(new NullInteropProject(name), _teamCityClient, _services, _channelStateBroadcaster));
            }
            return _projects[name.ToLower()];
        }

        private void Update()
        {
            var updated = new List<IProject>();
            var teamCityProjects = _teamCityClient.Projects.All();
            foreach (var teamCityProject in teamCityProjects.Where(teamCityProject => !_projects.ContainsKey(teamCityProject.Name.ToLower())))
            {
                _projects.Add(teamCityProject.Name, new Project(teamCityProject, _teamCityClient, _services, _channelStateBroadcaster));
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

        void IChannelConnectionStateListener.OnConnected()
        {
            var action = _onConnected;
            SetConnectedState();
            action();
        }

        void IChannelConnectionStateListener.OnDisconnected()
        {
            var action = _onDisconnected;
            SetDisconnectedState();
            action();
        }

        private void SetDisconnectedState()
        {
            _onDisconnected = () => { };
            _onConnected = () =>
            {
                _timerToken.Cancel();
                _timerToken = _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);
            };
        }

        private void SetConnectedState()
        {
            _onDisconnected = () =>
            {
                var token = _timerToken;
                _timerToken = new NullTimerToken();
                token.Cancel();
            };
            _onConnected = () => { };
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _timerToken = _services.Timer.QueueCallback(_updatePeriod, this);
        }
    }
}
