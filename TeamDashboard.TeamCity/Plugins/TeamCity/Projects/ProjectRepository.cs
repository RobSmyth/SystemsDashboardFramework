using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.DataSources.Jira;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public sealed class ProjectRepository : IChannelConnectionStateListener, ITimerListener, IProjectRepository
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromMinutes(1);
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly IServices _services;
        private Action _onConnected = () => { };
        private Action _onDisconnected = () => { };
        private ITimerToken _timerToken = new NullTimerToken();
        private readonly ILog _logger;
        private readonly TimeCachedArray<IProject> _projectCache;

        public ProjectRepository(IDataSource outerRepository, ITcSharpTeamCityClient teamCityClient, IServices services, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _teamCityClient = teamCityClient;
            _services = services;
            channelStateBroadcaster.Add(this);
            outerRepository.Write($"Agents.Count", 0);
            _logger = LogManager.GetLogger("Repositories.Projects");
            _projectCache = new TimeCachedArray<IProject>(() => _teamCityClient.Projects.All().Select(x => new TeamCityProject(x, _teamCityClient, _services)).ToArray(), TimeSpan.FromMinutes(5), _services.Clock);
            SetDisconnectedState();
        }

        public IProject[] GetAll()
        {
            return _projectCache.Items;
        }

        public IProject Get(string name)
        {
            var project = _projectCache.Items.SingleOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return project ?? new NullProject();
        }

        public Build[] GetRunningBuilds(string projectName, string buildConfigurationName)
        {
            _logger.DebugFormat("Request for running build: {0} / {1}.", projectName, buildConfigurationName);

            try
            {
                var project = _projectCache.Items.SingleOrDefault(x => x.Name.Equals(projectName));
                if (project == null)
                {
                    _logger.WarnFormat("Could not find project {0}.", projectName);
                    return new Build[0];
                }
                return project.GetRunningBuilds(buildConfigurationName);
            }
            catch (Exception exception)
            {
                _logger.Error("Exception while getting running build.", exception);
                return new Build[0];
            }
        }

        private void Update()
        {
            // todo
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
                _projectCache.StopWatching();
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
